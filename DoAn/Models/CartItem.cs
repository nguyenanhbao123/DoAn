namespace DoAn.Models
{
    public partial class CartItem
    {
        public string CartItemId { get; set; } = null!;

        public string? CartId { get; set; }

        public string? ProductId { get; set; }

        public int? Quantity { get; set; }

        public virtual Cart? Cart { get; set; }

        public virtual Product? Product { get; set; }
    }
}
