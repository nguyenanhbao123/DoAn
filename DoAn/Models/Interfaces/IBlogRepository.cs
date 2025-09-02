namespace DoAn.Models.Interfaces
{
    public interface IBlogRepository
    {
        IEnumerable<Blog> GetBlogs();
        Blog GetBlogById(string id);
        void AddBlog(Blog blog);
        void UpdateBlog(Blog blog);
        void DeleteBlog(string id);

        void SaveChanges();
        IEnumerable<Blog> SearchBlogs(string searchTerm);
    }
}
