namespace Depi_Project.Models.ViewModels
{

        public class GymBookingsViewModel
        {
            public Gym Gym { get; set; }
            public List<Booking> PendingBookings { get; set; }
            public List<Booking> ConfirmedBookings { get; set; }
            public List<Booking> CompletedBookings { get; set; }
            public List<Booking> CancelledBookings { get; set; }

            public int TotalBookings { get; set; }
            public decimal TotalRevenue { get; set; }
        }
}
