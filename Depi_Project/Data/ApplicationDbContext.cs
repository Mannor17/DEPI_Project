using Depi_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Depi_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<GymMedia> GymMedias { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Gym>()
                .HasOne(g => g.Owner)
                .WithOne(u => u.Gym)
                .HasForeignKey<Gym>(g => g.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Trainer>()
                .HasOne(t => t.Gym)
                .WithMany(g => g.Trainers)
                .HasForeignKey(t => t.GymId);

            builder.Entity<Review>()
                .HasOne(r => r.Gym)
                .WithMany(g => g.Reviews)
                .HasForeignKey(r => r.GymId);

            builder.Entity<Booking>()
                .HasOne(b => b.Gym)
                .WithMany(g => g.Bookings)
                .HasForeignKey(b => b.GymId);

            // Configure decimal precision for amounts
            builder.Entity<Booking>().Property(b => b.Amount).HasPrecision(10, 2);
        }
    }
}
