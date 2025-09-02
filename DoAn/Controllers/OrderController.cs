using DoAn.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DoAn.Data;

namespace DoAn.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly LuxDaDatabaseContext _context;
        public OrderController(IOrderRepository orderRepository, LuxDaDatabaseContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
        }
        public IActionResult OrderIndex(int page = 1)
        {
            int pageSize = 3;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var invoices = _orderRepository.GetPagedInvoices(userId, page, pageSize);
            int totalInvoices = _orderRepository.GetTotalInvoiceCount(userId);
            int totalPages = (int)Math.Ceiling((double)totalInvoices / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(invoices);
        }
        [HttpPost]
        public IActionResult CancelInvoice(string invoiceId)
        {
            var result = _orderRepository.CancelInvoice(invoiceId);

            if (result)
            {
                TempData["Success"] = "Đơn hàng đã được hủy.";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
            }

            return RedirectToAction("OrderIndex");
        }
        public IActionResult SortOrderStatus(string status, int page = 1)
        {
            int pageSize = 3;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Lấy toàn bộ hóa đơn
            var allInvoices = _orderRepository.GetOrdersByUserId(userId);

            // Đếm từng loại
            int totalAll = allInvoices.Count();
            int totalPending = allInvoices.Count(i => i.Status == "Đang xử lý");
            int totalConfirmed = allInvoices.Count(i => i.Status == "Đã xác nhận");
            int totalCanceled = allInvoices.Count(i => i.Status == "Đã hủy");

            // Lọc hóa đơn theo trạng thái (nếu có)
            var filteredInvoices = string.IsNullOrEmpty(status)
                ? allInvoices
                : allInvoices.Where(i => i.Status == status).ToList();

            int count = filteredInvoices.Count(); // Fix: Invoke the Count method
            int totalPages = (int)Math.Ceiling((double)count / pageSize);

            // Gửi dữ liệu sang View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SelectedStatus = status;

            ViewBag.CountAll = totalAll;
            ViewBag.CountPending = totalPending;
            ViewBag.CountConfirmed = totalConfirmed;
            ViewBag.CountCanceled = totalCanceled;

            return View("OrderIndex", filteredInvoices.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }
        [HttpPost]
        public IActionResult ConfirmInvoice(string invoiceId)
        {
            var result = _orderRepository.ConfirmInvoice(invoiceId);
            if (result)
            {
                TempData["Success"] = "Đơn hàng đã được xác nhận.";
            }
            else
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
            }
            return RedirectToAction("OrderIndex");
        }
    }
}
