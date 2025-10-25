using System.ComponentModel.DataAnnotations;

namespace Depi_Project.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int GymId { get; set; }
        public virtual Gym Gym { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        
        [Range(1, 5)] 
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
