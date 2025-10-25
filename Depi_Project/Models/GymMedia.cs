namespace Depi_Project.Models
{
    public class GymMedia
    {
        public int Id { get; set; }
        public int GymId { get; set; }
        public virtual Gym Gym { get; set; }
        public string Url { get; set; }
        public string Type { get; set; } // "Image" or "Video"
    }
}
