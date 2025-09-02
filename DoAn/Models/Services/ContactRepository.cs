using DoAn.Models.Interfaces;
using DoAn.Data;

namespace DoAn.Models.Services
{
    public class ContactRepository : IContactRepository
    {
        private readonly LuxDaDatabaseContext _context;
        public ContactRepository(LuxDaDatabaseContext context)
        {
            _context = context;
        }
        public void Add(ContactMessages message) => _context.ContactMessages.Add(message);

        public List<ContactMessages> GetLatest(int count = 5)
        {
            return _context.ContactMessages
                .OrderByDescending(m => m.SentAt)
                .Take(count)
                .ToList();
        }

        public int CountUnread() => _context.ContactMessages.Count(m => !m.IsRead);

        public void SaveChanges() => _context.SaveChanges();
    }
}
