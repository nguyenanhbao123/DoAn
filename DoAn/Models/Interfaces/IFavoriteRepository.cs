namespace DoAn.Models.Interfaces
{
    public interface IFavoriteRepository
    {
        IEnumerable<Product> GetFavoriteProducts(string userId);
        IEnumerable<Product> AddFavoriteProducts(string userId, string productId);
        IEnumerable<Product> RemoveFavoriteProducts(string userId, string productId);
    }
}
