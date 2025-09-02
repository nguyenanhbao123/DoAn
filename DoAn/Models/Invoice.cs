using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models
{
    public partial class Invoice
    {
        public string InvoiceId { get; set; } = null!;

        [ForeignKey("UserId")]
        public string? UserId { get; set; } = null!;

        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? DeliveryAddress { get; set; }

        public DateTime? CreatedDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Status { get; set; }


        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
        public virtual IdentityUser? User { get; set; }
    }
}
