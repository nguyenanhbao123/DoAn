using DoAn.Models;
using DoAn.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;
        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public IActionResult ContactIndex()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendContact(IFormCollection form)
        {
            var message = new ContactMessages
            {
                Name = form["Name"],
                Email = form["Email"],
                Phone = form["Phone"],
                Subject = form["Subject"],
                Message = form["Message"],
                SentAt = DateTime.Now,
                IsRead = false
            };

            _contactRepository.Add(message);
            _contactRepository.SaveChanges();

            return RedirectToAction("HomeIndex", "Home");
        }
    }
}
