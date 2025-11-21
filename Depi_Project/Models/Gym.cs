using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Depi_Project.Models
{
    public class Gym
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }   // for nearby search
        public double Longitude { get; set; }
        public string? OwnerId { get; set; }
        public decimal? PricePerDay { get; set; }
        public decimal? PricePerMonth { get; set; }
        public decimal? PriceWithTrainer { get; set; }
		public string? OpeningHours { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }


		public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<Trainer> Trainers { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<GymMedia> Media { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
