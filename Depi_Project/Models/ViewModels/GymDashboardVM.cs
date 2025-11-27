//namespace Depi_Project.Models.ViewModels
//{
//    public class GymDashboardVM
//    {
//        public Gym Gym { get; set; }
//        public List<Booking> RecentBookings { get; set; }

//        public int TotalBookings { get; set; }
//        public decimal TotalRevenue { get; set; }
//        public double AvgRating { get; set; }
//        public int ReviewsCount { get; set; }
//    }
//}

namespace Depi_Project.Models.ViewModels
{
    public class GymDashboardVM
    {
        public Gym? Gym { get; set; }
        public List<Booking>? RecentBookings { get; set; }
        public List<Booking>? PendingBookings { get; set; }

        public int TotalBookings { get; set; }
        public int PendingBookingsCount { get; set; }
        public int ConfirmedBookingsCount { get; set; }
        public int CancelledBookingsCount { get; set; }

        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }

        public double AvgRating { get; set; }
        public int ReviewsCount { get; set; }

        public int TotalTrainers { get; set; }
        public int MediaCount { get; set; }
    }
}

