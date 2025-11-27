using System.ComponentModel.DataAnnotations;


namespace Depi_Project.Models.ViewModels
    {
        public class EditGymViewModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Gym name is required")]
            [StringLength(100, MinimumLength = 3)]
            public string Name { get; set; }

            [Required(ErrorMessage = "Description is required")]
            [StringLength(1000, MinimumLength = 10)]
            public string Description { get; set; }

            [Required(ErrorMessage = "Address is required")]
            public string Address { get; set; }

            [Range(-90, 90)]
            public double Latitude { get; set; }

            [Range(-180, 180)]
            public double Longitude { get; set; }

            [Required]
            [Range(0.01, 10000)]
            public decimal PricePerDay { get; set; }

            [Required]
            [Range(0.01, 100000)]
            public decimal PricePerMonth { get; set; }

            [Range(0.01, 10000)]
            public decimal? PriceWithTrainer { get; set; }

            [Required]
            public string OpeningHours { get; set; }

            [Phone]
            public string? Phone { get; set; }

            [EmailAddress]
            public string? Email { get; set; }
        }
    }
