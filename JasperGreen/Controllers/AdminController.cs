using JasperGreen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JasperGreen.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly JasperGreenDbContext _context;

        public AdminController(JasperGreenDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            DateTime today = DateTime.Now;
            DateTime last30Days = today.AddDays(-30);
            DateTime previous30Days = today.AddDays(-60);

            decimal current30DayRevenue = _context.Payments
                .Where(p => p.Payment_Date >= last30Days && p.Payment_Date <= today)
                .Sum(p => (decimal?)p.Payment_Amount) ?? 0;

            decimal previous30DayRevenue = _context.Payments
                .Where(p => p.Payment_Date >= previous30Days && p.Payment_Date < last30Days)
                .Sum(p => (decimal?)p.Payment_Amount) ?? 0;

            decimal revenueChange = 0;
            if (previous30DayRevenue > 0)
            {
                revenueChange = ((current30DayRevenue - previous30DayRevenue) / previous30DayRevenue) * 100;
            }

            int weeklyServices = _context.Services
                .Count(s => s.Service_Date >= DateTime.Now.AddDays(-7));

            var monthlyRevenue = _context.Payments
                .GroupBy(p => new
                {
                    p.Payment_Date.Year,
                    p.Payment_Date.Month
                })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Take(6)
                .Select(g => new
                {
                    Label = $"{g.Key.Month}/{g.Key.Year}",
                    Revenue = g.Sum(x => x.Payment_Amount)
                })
                .ToList();

            var crewStats = _context.Services
                .Where(s => s.Service_Date >= DateTime.Now.AddDays(-7))
                .Include(s => s.Crew)
                    .ThenInclude(c => c.Foreman)
                .GroupBy(s => new
                {
                    s.Crew_ID,
                    ForemanName = s.Crew.Foreman.Emp_First_Name + " " + s.Crew.Foreman.Emp_Last_Name
                })
                .Select(g => new
                {
                    CrewName = g.Key.ForemanName,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            var recentActivities = _context.Services
                .Include(s => s.Crew)
                    .ThenInclude(c => c.Foreman)
                .Include(s => s.Property)
                .OrderByDescending(s => s.Service_Date)
                .Take(5)
                .Select(s => new ActivityItem
                {
                    Date = s.Service_Date,
                    CrewName = s.Crew.Foreman.Emp_First_Name + " " + s.Crew.Foreman.Emp_Last_Name,
                    Address = s.Property.Property_Address
                })
                .ToList();

            DashboardViewModel vm = new DashboardViewModel
            {
                RecentActivities = recentActivities,
                TotalCustomers = _context.Customers.Count(),
                TotalProperties = _context.Properties.Count(),
                TotalCrews = _context.Crews.Count(),
                TotalEmployees = _context.Employees.Count(),
                TotalRevenue = current30DayRevenue,
                WeeklyServices = weeklyServices,
                MonthlyRevenueChange = Math.Abs(revenueChange),
                RevenueIncreased = revenueChange >= 0,
                HasPreviousMonthData = previous30DayRevenue > 0,
                RevenueLabels = monthlyRevenue.Select(x => x.Label).ToList(),
                RevenueData = monthlyRevenue.Select(x => x.Revenue).ToList(),
                CrewLabels = crewStats.Select(x => x.CrewName).ToList(),
                CrewData = crewStats.Select(x => x.Count).ToList()
            };

            return View(vm);
        }

        public IActionResult Settings()
        {
            return View();
        }
    }
}
