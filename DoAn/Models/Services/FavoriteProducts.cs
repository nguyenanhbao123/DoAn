using DoAn.Models.Interfaces;
using DoAn.Data;

namespace DoAn.Models.Services
{
    public class FavoriteProducts : IFavoriteRepository
    {
        private readonly LuxDaDatabaseContext _context;
        public FavoriteProducts(LuxDaDatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> GetFavoriteProducts(string userId)
        {
            return _context.Favorites
                .Where(fp => fp.UserId == userId)
                .Select(fp => fp.Product)
                .ToList();
        }
        public IEnumerable<Product> AddFavoriteProducts(string userId, string productId)
        {
            var favorite = new Favorite
            {
                FavoriteId = "YT" + DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString("N").Substring(0, 4),
                UserId = userId,
                ProductId = productId
            };
            _context.Favorites.Add(favorite);
            _context.SaveChanges();
            return GetFavoriteProducts(userId);

        }
        public IEnumerable<Product> RemoveFavoriteProducts(string userId, string productId)
        {
            var favorite = _context.Favorites
                .FirstOrDefault(fp => fp.UserId == userId && fp.ProductId == productId);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
            }
            return GetFavoriteProducts(userId);
        }
    }
}
