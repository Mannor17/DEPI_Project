using System.ComponentModel.DataAnnotations.Schema;

namespace Depi_Project.Models
{
	public class Favorite
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public int GymId { get; set; }

		[ForeignKey("GymId")]
		public Gym Gym { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
	}
}
