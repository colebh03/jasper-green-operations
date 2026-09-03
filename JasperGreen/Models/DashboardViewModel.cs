using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JasperGreen.Models
{
    public class DashboardViewModel
    {
        public decimal TotalRevenue { get; set; }

        public int WeeklyServices { get; set; }       

        public decimal MonthlyRevenueChange { get; set; }

        public bool RevenueIncreased { get; set; }

        public bool HasPreviousMonthData { get; set; }

        // Current platform totals displayed on the dashboard
        public int TotalCustomers { get; set; }

        public int TotalProperties { get; set; }

        public int TotalCrews { get; set; }

        public int TotalEmployees { get; set; }

        // Most recent completed services displayed in the activity feed
        public List<ActivityItem> RecentActivities { get; set; } = new();
    }

    public class ActivityItem
    {
        public DateTime Date { get; set; }

        public string CrewName { get; set; }

        public string Address { get; set; }
    }
}