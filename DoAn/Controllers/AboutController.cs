using DoAn.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DoAn.Data;

namespace DoAn.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutRepository aboutRepository;
        private readonly LuxDaDatabaseContext luxDaDatabaseContext;
        public AboutController(IAboutRepository aboutRepository, LuxDaDatabaseContext luxDaDatabaseContext)
        {
            this.aboutRepository = aboutRepository;
            this.luxDaDatabaseContext = luxDaDatabaseContext;
        }

        public IActionResult AboutIndex()
        {
            var galleries = aboutRepository.GetGallery();
            return View(galleries);
        }
    }
}
