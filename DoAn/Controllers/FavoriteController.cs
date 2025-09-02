using DoAn.Data;
using DoAn.Models.Interfaces;
using DoAn.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoAn.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly LuxDaDatabaseContext _context;
        private readonly IFavoriteRepository favoriteProducts;
        public FavoriteController(LuxDaDatabaseContext context, IFavoriteRepository favoriteProducts)
        {
            _context = context;
            this.favoriteProducts = favoriteProducts;
        }
        public IActionResult FavoriteIndex()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");
            var favoriteProductsList = favoriteProducts.GetFavoriteProducts(userId);
            return View(favoriteProductsList);
        }
        [HttpGet]
        public IActionResult AddToFavorite(string productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                // Trả về JSON thay vì redirect
                return Json(new { redirectUrl = Url.Action("Login", "Account") });
            }

            var updatedFavorites = favoriteProducts.AddFavoriteProducts(userId, productId);
            return Json(new { success = true });
        }

        public IActionResult RemoveFromFavorite(string productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Json(new { redirectUrl = Url.Action("Login", "Account") });
            }

            var updatedFavorites = favoriteProducts.RemoveFavoriteProducts(userId, productId);
            return Ok();
        }
    }
}
