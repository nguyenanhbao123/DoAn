namespace DoAn.Models
{
    public partial class Blog
    {
        public string BlogId { get; set; } = null!;

        public string? Title { get; set; }

        public string? Content { get; set; }

        public string? Image { get; set; }

        public DateTime? PostedDate { get; set; }
    }
}
