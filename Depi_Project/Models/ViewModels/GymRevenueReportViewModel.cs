namespace Depi_Project.Models.ViewModels
{
    public class GymRevenueReportViewModel
    {
        public Gym Gym { get; set; }

        public decimal TotalRevenue { get; set; }
        public decimal DailyBookingsRevenue { get; set; }
        public decimal MonthlyBookingsRevenue { get; set; }
        public decimal TrainerBookingsRevenue { get; set; }

        public int TotalPaidBookings { get; set; }
        public int TotalUnpaidBookings { get; set; }

        public Dictionary<string, decimal> MonthlyRevenueChart { get; set; }
        public Dictionary<BookingType, int> BookingTypeDistribution { get; set; }
    }
}
