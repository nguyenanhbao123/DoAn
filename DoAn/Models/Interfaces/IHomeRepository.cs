namespace DoAn.Models.Interfaces
{
    public interface IHomeRepository
    {
        IEnumerable<Product> GetTrendingProducts();
    }
}
