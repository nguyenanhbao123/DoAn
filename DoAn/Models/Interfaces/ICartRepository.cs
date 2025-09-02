namespace DoAn.Models.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string? userId = null);

        // Thêm sản phẩm vào giỏ hàng
        Task AddItemToCartAsync(string productId, int quantity);

        // Xóa một sản phẩm cụ thể khỏi giỏ hàng
        Task RemoveItemFromCartAsync(string cartItemId);

        // Cập nhật số lượng của một sản phẩm trong giỏ hàng
        Task UpdateItemQuantityAsync(string cartItemId, int newQuantity);

        // Xóa toàn bộ giỏ hàng
        Task ClearCartAsync();

        // Lấy tổng giá trị của giỏ hàng
        Task<decimal> GetCartTotalAsync();

        // Xử lý thanh toán (chuyển đổi giỏ hàng thành đơn hàng)
        Task CheckoutAsync(string userId, ViewModel.CheckoutViewModel model);
    }
}
