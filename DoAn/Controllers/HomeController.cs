using DoAn.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DoAn.Data;

namespace DoAn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly LuxDaDatabaseContext _context;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, LuxDaDatabaseContext context, IHomeRepository homeRepository, ICartRepository cartRepository)
        {
            _logger = logger;
            _context = context;
            _homeRepository = homeRepository;
            _cartRepository = cartRepository;
        }
        public IActionResult HomeIndex()
        {
            var trendingProducts = _homeRepository.GetTrendingProducts();
            return View(trendingProducts);
        }
    }
}
