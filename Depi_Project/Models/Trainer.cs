using System.ComponentModel.DataAnnotations;

namespace Depi_Project.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        
        [Required] 
        public string Name { get; set; }
        public string Bio { get; set; }


		[Range(0, 5)]
		public double? Rating { get; set; }

		public int? SessionsCount { get; set; }
		public int GymId { get; set; }
        public virtual Gym Gym { get; set; }
    }
}
