namespace Depi_Project.Models.ViewModels
{
	public class UserDashboardViewModel
	{
		public string UserName { get; set; } = string.Empty;
		public int UpcomingBookings { get; set; }
		public int FavoriteGyms { get; set; }
		public decimal TotalSpent { get; set; }

		public List<Gym> Gyms { get; set; } = new();
		public List<Gym> Favorite { get; set; } = new();

	}
}
