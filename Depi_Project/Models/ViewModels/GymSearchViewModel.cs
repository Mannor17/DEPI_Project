using System.Collections.Generic;

namespace Depi_Project.Models.ViewModels
{
    public class GymSearchViewModel
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double? MinRating { get; set; }

        public List<Gym> Results { get; set; } = new List<Gym>();
    }
}
