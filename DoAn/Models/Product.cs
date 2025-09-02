namespace DoAn.Models
{
    public partial class Product
    {
        public string ProductId { get; set; } = null!;

        public string? ProductName { get; set; }

        public string? Image { get; set; }

        public int Price { get; set; }

        public decimal? AverageRating { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public bool Trending { get; set; }


        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    
    }
}
