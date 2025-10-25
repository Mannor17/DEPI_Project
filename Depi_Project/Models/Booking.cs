using System;
using System.ComponentModel.DataAnnotations;

public enum BookingType { ByDay, ByMonth, WithTrainer }

namespace Depi_Project.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int GymId { get; set; }
        public virtual Gym Gym { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public BookingType Type { get; set; }
        public DateTime StartDate { get; set; }  // date/time chosen
        public DateTime? EndDate { get; set; }   // for monthly
        public int? TrainerId { get; set; }      // if WithTrainer
        public virtual Trainer Trainer { get; set; }

        public decimal Amount { get; set; }
        public bool IsPaid { get; set; } = false;
        public bool IsConfirmedByOwner { get; set; } = false;
        public bool IsCancelled { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string PaymentReceiptUrl { get; set; }
    }
}
