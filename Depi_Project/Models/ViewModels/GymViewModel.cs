namespace Depi_Project.Models.ViewModels
{
	public class GymViewModel
	{
		public string Name { get; set; } = string.Empty;
		public string Location { get; set; } = string.Empty;
		public double Rating { get; set; }
		public decimal Price { get; set; }
		public string ImageUrl { get; set; } = string.Empty;
		public List<string> Features { get; set; } = new();
	}
}
