namespace Depi_Project.Models.ViewModels
{
    public class CreateBookingViewModel
    {
		public int GymId { get; set; }
		public string GymName { get; set; }

		public BookingType Type { get; set; }

		public DateTime SelectedDate { get; set; }
		public string SelectedTime { get; set; }

		public decimal Price { get; set; }

		public List<GymSessionOption> SessionOptions { get; set; } = new();
		public List<Trainer> Trainers { get; set; }

	}
	public class GymSessionOption
	{
		public BookingType Type { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Description { get; set; }
	}
}
