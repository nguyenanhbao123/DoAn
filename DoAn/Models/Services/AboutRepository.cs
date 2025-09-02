using DoAn.Models.Interfaces;
using DoAn.Data;

namespace DoAn.Models.Services
{
    public class AboutRepository : IAboutRepository
    {
        private readonly LuxDaDatabaseContext _context;
        public AboutRepository(LuxDaDatabaseContext context)
        {
            _context = context;
        }
        IEnumerable<Gallery> IAboutRepository.GetGallery()
        {
            return _context.Galleries.ToList();
        }
        public Gallery GetById(string id) => _context.Galleries.Find(id);

        public void Add(Gallery image)
        {
            _context.Galleries.Add(image);
        }

        public void Update(Gallery image)
        {
            _context.Galleries.Update(image);
        }

        public void Delete(string id)
        {
            var image = GetById(id);
            if (image != null)
            {
                _context.Galleries.Remove(image);
            }
        }
        public string GenerateNextGalleryId()
        {
            var lastId = _context.Galleries
                .OrderByDescending(g => g.GalleryId)
                .Select(g => g.GalleryId)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(lastId) && lastId.Length >= 4 && lastId.StartsWith("G"))
            {
                // Cắt phần số, tăng lên 1
                int numberPart = int.Parse(lastId.Substring(1));
                return $"G{(numberPart + 1):D3}"; // Format 3 chữ số
            }

            return "G001"; // Nếu chưa có ảnh nào
        }


        public void Save() => _context.SaveChanges();
    }
}
