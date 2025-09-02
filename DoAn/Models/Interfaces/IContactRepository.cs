namespace DoAn.Models.Interfaces
{
    public interface IContactRepository
    {
        void Add(ContactMessages message);
        List<ContactMessages> GetLatest(int count = 5);
        int CountUnread();
        void SaveChanges();
    }
}
