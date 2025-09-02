namespace DoAn.Models
{
    public class OrderNotification
    {
        public string OrderId { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? UserId { get; set; } = null!;

        public decimal? TotalAmount { get; set; }
    }
}
