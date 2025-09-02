namespace DoAn.Models.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Invoice> GetOrdersByUserId(string userId);
        IEnumerable<Invoice> GetPagedInvoices(string userId, int page, int pageSize);
        int GetTotalInvoiceCount(string userId);

        public bool CancelInvoice(string invoiceId);

        public bool ConfirmInvoice(string invoiceId);

        IEnumerable<Invoice> GetAllInvoicesbyStatus(string userId, string status);

        IEnumerable<Invoice> GetAllInvoices();
    }
}
