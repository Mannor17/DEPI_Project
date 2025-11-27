using System.ComponentModel.DataAnnotations;

namespace Depi_Project.Models.ViewModels
{
    public class CreateGymViewModel
    {
        [Required(ErrorMessage = "Gym name is required")]
        [StringLength(100, MinimumLength = 3)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, MinimumLength = 10)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "Price per day is required")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000")]
        public decimal PricePerDay { get; set; }

        [Required(ErrorMessage = "Price per month is required")]
        [Range(0.01, 100000, ErrorMessage = "Price must be between 0.01 and 100000")]
        public decimal PricePerMonth { get; set; }

        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000")]
        public decimal? PriceWithTrainer { get; set; }

        [Required(ErrorMessage = "Opening hours are required")]
        public string? OpeningHours { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
    }
}
