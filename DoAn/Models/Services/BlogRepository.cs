using DoAn.Models.Interfaces;
using DoAn.Data;

namespace DoAn.Models.Services

{
    public class BlogRepository : IBlogRepository
    {
        private readonly LuxDaDatabaseContext _context;
        public BlogRepository(LuxDaDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Blog> GetBlogs()
        {
            return _context.Blogs.ToList();
        }

        public Blog GetBlogById(string id)
        {
            return _context.Blogs.FirstOrDefault(b => b.BlogId == id);
        }

        public void AddBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            _context.SaveChanges();
        }

        public void UpdateBlog(Blog blog)
        {
            _context.Blogs.Update(blog);
            _context.SaveChanges();
        }

        public void DeleteBlog(string id)
        {
            var blog = GetBlogById(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                _context.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Blog> SearchBlogs(string searchTerm)
        {
            return _context.Blogs.Where(b => b.Title.Contains(searchTerm) || b.Content.Contains(searchTerm)).ToList();
        }
    }
}
