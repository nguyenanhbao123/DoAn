using DoAn.Models.Interfaces;
using DoAn.Models.ViewModel;
using DoAn.Data;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Models.Services
{
    public class CartRepository : ICartRepository
    {
        private readonly LuxDaDatabaseContext _context; // Thay YourDbContext bằng tên DbContext thực tế của bạn
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "CartId"; // Key để lưu CartId trong session

        public CartRepository(LuxDaDatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // --- Hàm hỗ trợ quản lý Session ---
        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        private string GetCartId()
        {
            if (Session.GetString(CartSessionKey) == null)
            {
                // Tạo một Cart ID mới và lưu vào session nếu chưa có
                Guid newCartId = Guid.NewGuid();
                Session.SetString(CartSessionKey, newCartId.ToString());
                return newCartId.ToString();
            }
            return Session.GetString(CartSessionKey)!;
        }

        // --- Các thao tác chính của Giỏ hàng ---

        public async Task<Cart> GetCartAsync(string? userId = null)
        {
            string cartId = GetCartId(); // Lấy Cart ID từ session cho người dùng ẩn danh

            Cart? cart;
            if (!string.IsNullOrEmpty(userId))
            {
                // Thử tìm giỏ hàng liên kết với User ID
                cart = await _context.Carts
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                // Nếu người dùng có giỏ hàng, chuyển các mục từ giỏ hàng ẩn danh sang
                if (cart != null)
                {
                    var anonymousCart = await _context.Carts
                        .Include(c => c.CartItems)
                            .ThenInclude(ci => ci.Product)
                        .FirstOrDefaultAsync(c => c.CartId == cartId && c.UserId == null);

                    if (anonymousCart != null)
                    {
                        foreach (var anonymousItem in anonymousCart.CartItems)
                        {
                            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == anonymousItem.ProductId);
                            if (existingItem != null)
                            {
                                existingItem.Quantity += anonymousItem.Quantity;
                            }
                            else
                            {
                                cart.CartItems.Add(new CartItem
                                {
                                    CartItemId = Guid.NewGuid().ToString(),
                                    ProductId = anonymousItem.ProductId,
                                    Quantity = anonymousItem.Quantity
                                });
                            }
                            _context.CartItems.Remove(anonymousItem); // Xóa mục khỏi giỏ hàng ẩn danh
                        }
                        _context.Carts.Remove(anonymousCart); // Xóa giỏ hàng ẩn danh
                        await _context.SaveChangesAsync();
                    }
                    // Cập nhật session để phản ánh Cart ID của người dùng
                    Session.SetString(CartSessionKey, cart.CartId);
                    return cart;
                }
            }

            // Nếu không có User ID hoặc không có giỏ hàng cụ thể cho người dùng, lấy hoặc tạo giỏ hàng ẩn danh
            cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart == null)
            {
                cart = new Cart { CartId = cartId, UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }

        public async Task AddItemToCartAsync(string productId, int quantity)
        {
            string cartId = GetCartId();
            var cart = await GetCartAsync(); // Hàm này sẽ đảm bảo giỏ hàng tồn tại và được liên kết với session
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                // Đảm bảo Product tồn tại trước khi thêm vào CartItem (tùy chọn, nhưng là thực hành tốt)
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {productId} not found.");
                }

                cart.CartItems.Add(new CartItem
                {
                    CartItemId = Guid.NewGuid().ToString(), // Tạo ID duy nhất cho CartItem
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemFromCartAsync(string cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateItemQuantityAsync(string cartItemId, int newQuantity)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                if (newQuantity > 0)
                {
                    cartItem.Quantity = newQuantity;
                }
                else
                {
                    _context.CartItems.Remove(cartItem); // Xóa nếu số lượng là 0 hoặc ít hơn
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync()
        {
            string cartId = GetCartId();
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                _context.Carts.Remove(cart); // Xóa cả giỏ hàng
                Session.Remove(CartSessionKey); // Xóa Cart ID khỏi session
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetCartTotalAsync()
        {
            var cart = await GetCartAsync();
            decimal total = 0;
            foreach (var item in cart.CartItems)
            {
                // Đảm bảo Product và Quantity không null trước khi tính toán
                if (item.Product != null && item.Quantity.HasValue)
                {
                    total += item.Product.Price * item.Quantity.Value; // Giả định Product có thuộc tính Price
                }
            }
            return total;
        }

        // --- Liên quan đến Thanh toán (ví dụ cơ bản) ---
        public async Task CheckoutAsync(string? userId, CheckoutViewModel model)
        {
            var cart = await GetCartAsync(userId);
            if (!cart.CartItems.Any()) return;

            var invoice = new Invoice
            {
                InvoiceId = Guid.NewGuid().ToString(),
                UserId = userId,
                CreatedDate = DateTime.Now,
                TotalAmount = await GetCartTotalAsync(),
                Status = "Đang xử lý",

                // Lưu thêm thông tin người nhận từ model
                CustomerName = model.FullName,
                Phone = model.Phone,
                DeliveryAddress = $"{model.Address}, {model.WardName}, {model.DistrictName}, {model.ProvinceName}"

,
            };

            _context.Invoices.Add(invoice);

            foreach (var item in cart.CartItems)
            {
                _context.InvoiceDetails.Add(new InvoiceDetail
                {
                    InvoiceDetailId = Guid.NewGuid().ToString(),
                    InvoiceId = invoice.InvoiceId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price,
                    Note = model.Note,
                });
            }

            await ClearCartAsync();
            await _context.SaveChangesAsync();
        }

    }
}
