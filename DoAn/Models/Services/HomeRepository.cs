using DoAn.Models.Interfaces;
using DoAn.Data;

namespace DoAn.Models.Services
{
    public class HomeRepository : IHomeRepository
    {
        private readonly LuxDaDatabaseContext _context;
        public HomeRepository(LuxDaDatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> GetTrendingProducts()
        {
            return _context.Products
                .Where(p => p.Trending == true)
                .OrderBy(p => Guid.NewGuid()) // Sắp xếp ngẫu nhiên
                .Take(3)
                .ToList();
        }
    }
}
