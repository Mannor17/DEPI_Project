namespace Depi_Project.Models.ViewModels
{
	public class GymDetailsViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public double Rating { get; set; }
		public int ReviewsCount { get; set; }
		public string? OpeningHours { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }

		// صور الميديا (رابط الصورة)
		public List<string> Images { get; set; } = new();

		// إذا عندك كلاس Review و Trainer في المشروع:
		public List<Review> Reviews { get; set; } = new();
		public List<Trainer> Trainers { get; set; } = new();
	}
}
