namespace DoAn.Models
{
    public partial class InvoiceDetail
    {
        public string InvoiceDetailId { get; set; } = null!;

        public string? InvoiceId { get; set; }

        public string? ProductId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public string? Note { get; set; }


        public virtual Invoice? Invoice { get; set; }

        public virtual Product? Product { get; set; }
    }
}
