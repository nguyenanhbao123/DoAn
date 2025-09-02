using DoAn.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DoAn.Models.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartRepository _cartService; // Sử dụng interface

        public CartViewComponent(ICartRepository cartService) // Tiêm ICartService
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string? userId = null;

            if (User.Identity!.IsAuthenticated)
            {
                // Cast User.Identity to ClaimsIdentity to access FindFirst
                var claimsIdentity = User.Identity as ClaimsIdentity;
                userId = claimsIdentity?.FindFirst("UserID")?.Value;

                // Nếu không tìm được userId hợp lệ thì vẫn dùng session
                if (string.IsNullOrWhiteSpace(userId))
                {
                    userId = null;
                }
            }

            var cart = await _cartService.GetCartAsync(userId); // userId = null nếu chưa đăng nhập hoặc không có claim hợp lệ

            ViewBag.CartTotal = await _cartService.GetCartTotalAsync();
            return View(cart);
        }
    }
}
