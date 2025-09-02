namespace DoAn.Models.Interfaces
{
    public interface IShopRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);

        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(string productId);
    }
}
