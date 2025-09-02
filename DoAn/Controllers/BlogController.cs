using DoAn.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DoAn.Data;

namespace DoAn.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository blogRepository;
        private readonly LuxDaDatabaseContext luxDaDatabaseContext;
        public BlogController(IBlogRepository blogRepository, LuxDaDatabaseContext luxDaDatabaseContext)
        {
            this.blogRepository = blogRepository;
            this.luxDaDatabaseContext = luxDaDatabaseContext;
        }
        public IActionResult BlogIndex()
        {
            var blogs = blogRepository.GetBlogs();
            return View(blogs);
        }
    }
}
