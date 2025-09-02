namespace DoAn.Models.Interfaces
{
    public interface IAboutRepository
    {
        IEnumerable<Gallery> GetGallery();
        Gallery GetById(string id);
        void Add(Gallery image);
        void Update(Gallery image);
        void Delete(string id);
        void Save();
        string GenerateNextGalleryId();
    }
}

