using DoAn.Data;
using DoAn.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DoAn.Models.Services
{
    public class AdminRepository : IAdminRepository
    {
        private readonly LuxDaDatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminRepository(LuxDaDatabaseContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Dictionary<int, int> GetMonthlyProductSold(int year)
        {
            return _context.InvoiceDetails
                .Where(d => d.Invoice != null && d.Invoice.CreatedDate.HasValue && d.Invoice.CreatedDate.Value.Year == year)
                .GroupBy(d => d.Invoice.CreatedDate.Value.Month)
                .Select(g => new { Month = g.Key, Quantity = g.Sum(x => x.Quantity ?? 0) })
                .ToDictionary(g => g.Month, g => g.Quantity);
        }

        public Dictionary<int, decimal> GetMonthlyRevenue(int year)
        {
            return _context.Invoices
                .Where(i => i.CreatedDate.HasValue && i.CreatedDate.Value.Year == year)
                .GroupBy(i => i.CreatedDate.Value.Month)
                .Select(g => new { Month = g.Key, Revenue = g.Sum(x => x.TotalAmount ?? 0) })
                .ToDictionary(g => g.Month, g => g.Revenue);
        }

        public List<ProductStat> GetTopProducts(int top)
        {
            return _context.InvoiceDetails
                .Where(d => d.Product != null)
                .GroupBy(d => d.Product.ProductName)
                .Select(g => new ProductStat
                {
                    ProductName = g.Key,
                    TotalQuantity = g.Sum(x => x.Quantity ?? 0)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(top)
                .ToList();
        }


        public decimal GetTotalRevenue(int year)
        {
            return _context.Invoices
                .Where(i => i.CreatedDate.HasValue && i.CreatedDate.Value.Year == year)
                .Sum(i => i.TotalAmount ?? 0);
        }

        public int GetTotalSoldProducts(int year)
        {
            return _context.InvoiceDetails
                .Where(d => d.Invoice != null && d.Invoice.CreatedDate.HasValue && d.Invoice.CreatedDate.Value.Year == year)
                .Sum(d => d.Quantity ?? 0);
        }

        public int GetTotalSuccessfulOrders(int year)
        {
            return _context.Invoices
                .Count(i => i.Status == "Đã xác nhận" && i.CreatedDate.HasValue && i.CreatedDate.Value.Year == year);
        }

        public int GetTotalCustomers()
        {
            return _context.Users.Count();
        }
        public List<IdentityUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        public async Task<IdentityUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IList<string>> GetUserRolesAsync(IdentityUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task UpdateUserRolesAsync(IdentityUser user, IEnumerable<string> roles)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task<List<string>> GetAllRolesAsync()
        {
            return await Task.FromResult(_roleManager.Roles.Select(r => r.Name!).ToList());
        }
        public List<OrderNotification> GetRecentUnprocessedOrders(int limit = 5)
        {
            return _context.Invoices
                .Where(i => i.Status == "Chưa xử lý")
                .OrderByDescending(i => i.CreatedDate)
                .Take(limit)
                .Select(i => new OrderNotification
                {
                    OrderId = i.InvoiceId,
                    UserId = i.UserId,
                    TotalAmount = i.TotalAmount,
                    CreatedAt = i.CreatedDate ?? DateTime.Now
                })
                .ToList();
        }
    }
}
