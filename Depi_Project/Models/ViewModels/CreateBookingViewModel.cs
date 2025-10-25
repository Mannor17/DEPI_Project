namespace Depi_Project.Models.ViewModels
{
    public class CreateBookingViewModel
    {
        public int GymId { get; set; }
        public BookingType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TrainerId { get; set; }
        public decimal Amount { get; set; }
    }
}
