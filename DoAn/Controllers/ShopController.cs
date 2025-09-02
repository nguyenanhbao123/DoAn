using DoAn.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DoAn.Data;

namespace DoAn.Controllers
{
    public class ShopController : Controller
    {
        private readonly IShopRepository _shopRepository;
        private readonly LuxDaDatabaseContext _context;
        public ShopController(IShopRepository shopRepository, LuxDaDatabaseContext context)
        {
            _shopRepository = shopRepository;
            _context = context;
        }
        public IActionResult ShopIndex()
        {
            var products = _shopRepository.GetAllProducts();
            return View(products);
        }
        public IActionResult ShopCategory(string category)
        {
            var products = _shopRepository.GetProductsByCategory(category);
            return View("ShopIndex", products);
        }
        public IActionResult ShopDetail(string id)
        {
            var product = _context.Products
             .FirstOrDefault(p => p.ProductId == id);

            if (product == null) return NotFound();

            return View(product);
        }
    }
}
