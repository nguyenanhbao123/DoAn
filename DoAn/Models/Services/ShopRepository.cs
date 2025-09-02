using DoAn.Models.Interfaces;
using DoAn.Data;

namespace DoAn.Models.Services
{
    public class ShopRepository : IShopRepository
    {
        private readonly LuxDaDatabaseContext _context;
        private readonly IShopRepository _shoprepository;
        public ShopRepository(LuxDaDatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _context.Products
                .Where(p => p.Category == category)
                .ToList();
        }
        public void AddProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.ProductId))
            {
                product.ProductId = "SP" + DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString("N")[..4];
            }

            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            if (product.AverageRating == null)
            {
                product.AverageRating = _context.Products
                    .Where(p => p.ProductId == product.ProductId)
                    .Select(p => p.AverageRating)
                    .FirstOrDefault();
            }
            _context.Products.Update(product);
            _context.SaveChanges();
        }
        public void DeleteProduct(string productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

        }
    }
}
