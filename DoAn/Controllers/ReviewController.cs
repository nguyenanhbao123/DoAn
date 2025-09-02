using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
