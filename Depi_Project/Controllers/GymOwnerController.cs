
//namespace Depi_Project.Controllers
//{
//    [Authorize(Roles = "GymOwner")]
//    public class GymOwnerController : Controller
//    {
//        private readonly ApplicationDbContext _db;
//        public GymOwnerController(ApplicationDbContext db) { _db = db; }

//        public async Task<IActionResult> Dashboard()
//        {
//            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            var gym = await _db.Gyms
//                .Include(g => g.Bookings)
//                .Include(g => g.Reviews)
//                .Include(g => g.Media)
//                .FirstOrDefaultAsync(g => g.OwnerId == ownerId);

//            var recentBooking = gym.Bookings
//                .OrderByDescending(b=>b.CreatedAt)
//                .Take(5)
//                .ToList();

//            var totalBooking = gym.Bookings.Count();

//            var totalRevenue = gym.Bookings
//                .Where(b=>b.IsPaid)
//                .Sum(b=>b.Amount);

//            var avgRating = gym.Reviews.Any() ? gym.Reviews
//                .Average(r => r.Rating) : 0;

//            var model = new GymDashboardVM
//            {
//                Gym = gym,
//                RecentBookings = recentBooking,
//                TotalBookings = totalBooking,
//                TotalRevenue = totalRevenue,
//                AvgRating = avgRating,
//                ReviewsCount = gym.Reviews.Count()
//            };

//            return View(model);
//        }

//        public IActionResult Bookings() => View();
//        public IActionResult Media() => View();

//        public async Task<IActionResult> ApproveBooking(int id)
//        {
//            var booking = await _db.Bookings.Include(b => b.Gym)
//                                            .FirstOrDefaultAsync(b => b.Id == id);
//            if (booking == null) return NotFound();
//            if (booking.Gym.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Forbid();

//            booking.IsConfirmedByOwner = true;
//            await _db.SaveChangesAsync();
//            return RedirectToAction("Dashboard");
//        }

//        public async Task<IActionResult> RejectBooking(int id)
//        {
//            var booking = await _db.Bookings.Include(b => b.Gym)
//                                            .FirstOrDefaultAsync(b => b.Id == id);
//            if (booking == null) return NotFound();
//            if (booking.Gym.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Forbid();

//            booking.IsCancelled = true;
//            await _db.SaveChangesAsync();
//            return RedirectToAction("Dashboard");
//        }

//        [HttpPost]
//        public async Task<IActionResult> UploadMedia(int gymId, IFormFile file, string type)
//        {
//            var url = await SaveFileAsync(file);
//            _db.GymMedias.Add(new GymMedia { GymId = gymId, Url = url, Type = type });
//            await _db.SaveChangesAsync();
//            return RedirectToAction("Dashboard");
//        }

//        private async Task<string> SaveFileAsync(IFormFile file)
//        {
//            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
//            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
//            using (var stream = new FileStream(path, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }
//            return "/uploads/" + fileName;
//        }
//    }
//}


using Depi_Project.Data;
using Depi_Project.Models;
using Depi_Project.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Depi_Project.Controllers
{
    [Authorize(Roles = "GymOwner")]
    public class GymOwnerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public GymOwnerController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        // ===================== DASHBOARD =====================
        public async Task<IActionResult> Dashboard()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var gym = await _db.Gyms
                .Include(g => g.Bookings).ThenInclude(b => b.User)
                .Include(g => g.Bookings).ThenInclude(b => b.Trainer)
                .Include(g => g.Reviews).ThenInclude(r => r.User)
                .Include(g => g.Media)
                .Include(g => g.Trainers)
                .FirstOrDefaultAsync(g => g.OwnerId == ownerId);

            if (gym == null)
            {
                // No gym created yet - redirect to create
                return RedirectToAction("Create");
            }

            var recentBookings = gym.Bookings
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .ToList();

            var pendingBookings = gym.Bookings
                .Where(b => !b.IsConfirmedByOwner && !b.IsCancelled)
                .OrderBy(b => b.StartDate)
                .ToList();

            var totalBookings = gym.Bookings.Count();
            var confirmedBookings = gym.Bookings.Count(b => b.IsConfirmedByOwner);
            var cancelledBookings = gym.Bookings.Count(b => b.IsCancelled);

            var totalRevenue = gym.Bookings
                .Where(b => b.IsPaid && b.IsConfirmedByOwner)
                .Sum(b => b.Amount);

            var monthlyRevenue = gym.Bookings
                .Where(b => b.IsPaid && b.IsConfirmedByOwner && b.CreatedAt.Month == DateTime.UtcNow.Month)
                .Sum(b => b.Amount);

            var avgRating = gym.Reviews.Any() ? gym.Reviews.Average(r => r.Rating) : 0;

            var model = new GymDashboardVM
            {
                Gym = gym,
                RecentBookings = recentBookings,
                PendingBookings = pendingBookings,
                TotalBookings = totalBookings,
                PendingBookingsCount = pendingBookings.Count,
                ConfirmedBookingsCount = confirmedBookings,
                CancelledBookingsCount = cancelledBookings,
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue,
                AvgRating = avgRating,
                ReviewsCount = gym.Reviews.Count(),
                TotalTrainers = gym.Trainers.Count(),
                MediaCount = gym.Media.Count()
            };

            return View(model);
        }

        // ===================== CREATE GYM =====================
        [HttpGet]
        public IActionResult Create()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if gym already exists
            var existingGym = _db.Gyms.FirstOrDefault(g => g.OwnerId == ownerId);
            if (existingGym != null)
            {
                return RedirectToAction("Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGymViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if gym already exists
            var existingGym = await _db.Gyms.FirstOrDefaultAsync(g => g.OwnerId == ownerId);
            if (existingGym != null)
            {
                ModelState.AddModelError("", "You already have a gym registered.");
                return View(model);
            }

            var gym = new Gym
            {
                Name = model.Name,
                Description = model.Description,
                Address = model.Address,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                PricePerDay = model.PricePerDay,
                PricePerMonth = model.PricePerMonth,
                PriceWithTrainer = model.PriceWithTrainer,
                OpeningHours = model.OpeningHours,
                Phone = model.Phone,
                Email = model.Email,
                OwnerId = ownerId
            };

            _db.Gyms.Add(gym);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Gym created successfully!";
            return RedirectToAction("Dashboard");
        }

        // ===================== EDIT GYM =====================
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gym = await _db.Gyms.FirstOrDefaultAsync(g => g.OwnerId == ownerId);

            if (gym == null)
            {
                return NotFound();
            }

            var model = new EditGymViewModel
            {
                Id = gym.Id,
                Name = gym.Name,
                Description = gym.Description,
                Address = gym.Address,
                Latitude = gym.Latitude,
                Longitude = gym.Longitude,
                PricePerDay = gym.PricePerDay ?? 0,
                PricePerMonth = gym.PricePerMonth ?? 0,
                PriceWithTrainer = gym.PriceWithTrainer,
                OpeningHours = gym.OpeningHours,
                Phone = gym.Phone,
                Email = gym.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGymViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gym = await _db.Gyms.FirstOrDefaultAsync(g => g.Id == model.Id && g.OwnerId == ownerId);

            if (gym == null)
            {
                return NotFound();
            }

            gym.Name = model.Name;
            gym.Description = model.Description;
            gym.Address = model.Address;
            gym.Latitude = model.Latitude;
            gym.Longitude = model.Longitude;
            gym.PricePerDay = model.PricePerDay;
            gym.PricePerMonth = model.PricePerMonth;
            gym.PriceWithTrainer = model.PriceWithTrainer;
            gym.OpeningHours = model.OpeningHours;
            gym.Phone = model.Phone;
            gym.Email = model.Email;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Gym updated successfully!";
            return RedirectToAction("Dashboard");
        }

        // ===================== BOOKINGS MANAGEMENT =====================
        [HttpGet]
        public async Task<IActionResult> Bookings()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gym = await _db.Gyms
                .Include(g => g.Bookings).ThenInclude(b => b.User)
                .Include(g => g.Bookings).ThenInclude(b => b.Trainer)
                .FirstOrDefaultAsync(g => g.OwnerId == ownerId);

            if (gym == null)
            {
                return NotFound();
            }

            var model = new GymBookingsViewModel
            {
                Gym = gym,
                PendingBookings = gym.Bookings
                    .Where(b => !b.IsConfirmedByOwner && !b.IsCancelled)
                    .OrderBy(b => b.StartDate)
                    .ToList(),
                ConfirmedBookings = gym.Bookings
                    .Where(b => b.IsConfirmedByOwner && !b.IsCancelled && b.StartDate >= DateTime.UtcNow)
                    .OrderBy(b => b.StartDate)
                    .ToList(),
                CompletedBookings = gym.Bookings
                    .Where(b => b.IsConfirmedByOwner && !b.IsCancelled && b.StartDate < DateTime.UtcNow)
                    .OrderByDescending(b => b.StartDate)
                    .ToList(),
                CancelledBookings = gym.Bookings
                    .Where(b => b.IsCancelled)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToList(),
                TotalBookings = gym.Bookings.Count(),
                TotalRevenue = gym.Bookings.Where(b => b.IsPaid && b.IsConfirmedByOwner).Sum(b => b.Amount)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveBooking(int id)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var booking = await _db.Bookings
                .Include(b => b.Gym)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            if (booking.Gym.OwnerId != ownerId)
            {
                return Forbid();
            }

            booking.IsConfirmedByOwner = true;
            await _db.SaveChangesAsync();

            TempData["Success"] = "Booking approved successfully!";
            return RedirectToAction("Bookings");
        }

        [HttpPost]
        public async Task<IActionResult> RejectBooking(int id)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var booking = await _db.Bookings
                .Include(b => b.Gym)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            if (booking.Gym.OwnerId != ownerId)
            {
                return Forbid();
            }

            booking.IsCancelled = true;
            await _db.SaveChangesAsync();

            TempData["Success"] = "Booking rejected successfully!";
            return RedirectToAction("Bookings");
        }

        // ===================== TRAINERS MANAGEMENT =====================
        [HttpGet]
        public async Task<IActionResult> Trainers()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gym = await _db.Gyms
                .Include(g => g.Trainers)
                .FirstOrDefaultAsync(g => g.OwnerId == ownerId);

            if (gym == null)
            {
                return NotFound();
            }

            var model = new ManageTrainersViewModel
            {
                Gym = gym,
                Trainers = gym.Trainers.ToList(),
                NewTrainer = new CreateTrainerViewModel { GymId = gym.Id }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainer(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid trainer data.";
                return RedirectToAction("Trainers");
            }

            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gym = await _db.Gyms.FirstOrDefaultAsync(g => g.Id == model.GymId && g.OwnerId == ownerId);

            if (gym == null)
            {
                return Forbid();
            }

            var trainer = new Trainer
            {
                Name = model.Name,
                Bio = model.Bio,
                GymId = model.GymId,
                Rating = 0,
                SessionsCount = 0
            };

            _db.Trainers.Add(trainer);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Trainer added successfully!";
            return RedirectToAction("Trainers");
        }

        [HttpGet]
        public async Task<IActionResult> EditTrainer(int id)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var trainer = await _db.Trainers
                .Include(t => t.Gym)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null || trainer.Gym.OwnerId != ownerId)
            {
                return NotFound();
            }

            var model = new EditTrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Bio = trainer.Bio,
                GymId = trainer.GymId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainer(EditTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var trainer = await _db.Trainers
                .Include(t => t.Gym)
                .FirstOrDefaultAsync(t => t.Id == model.Id);

            if (trainer == null || trainer.Gym.OwnerId != ownerId)
            {
                return NotFound();
            }

            trainer.Name = model.Name;
            trainer.Bio = model.Bio;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Trainer updated successfully!";
            return RedirectToAction("Trainers");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var trainer = await _db.Trainers
                .Include(t => t.Gym)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null || trainer.Gym.OwnerId != ownerId)
            {
                return NotFound();
            }

            // Check if trainer has bookings
            var hasBookings = await _db.Bookings.AnyAsync(b => b.TrainerId == id);
            if (hasBookings)
            {
                TempData["Error"] = "Cannot delete trainer with existing bookings.";
                return RedirectToAction("Trainers");
            }

            _db.Trainers.Remove(trainer);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Trainer deleted successfully!";
            return RedirectToAction("Trainers");
        }

        // ===================== MEDIA MANAGEMENT =====================
        [HttpGet]
        public async Task<IActionResult> Media()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gym = await _db.Gyms
                .Include(g => g.Media)
                .FirstOrDefaultAsync(g => g.OwnerId == ownerId);

            if (gym == null)
            {
                return NotFound();
            }

            var model = new ManageMediaViewModel
            {
                Gym = gym,
                Images = gym.Media.Where(m => m.Type == "Image").ToList(),
                Videos = gym.Media.Where(m => m.Type == "Video").ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadMedia(int gymId, IFormFile file, string type)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a file to upload.";
                return RedirectToAction("Media");
            }

            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gym = await _db.Gyms.FirstOrDefaultAsync(g => g.Id == gymId && g.OwnerId == ownerId);

            if (gym == null)
            {
                return Forbid();
            }

            // Validate file type
            var allowedExtensions = type == "Image"
                ? new[] { ".jpg", ".jpeg", ".png", ".gif" }
                : new[] { ".mp4", ".avi", ".mov" };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                TempData["Error"] = $"Invalid file type. Allowed: {string.Join(", ", allowedExtensions)}";
                return RedirectToAction("Media");
            }

            // Validate file size (10MB for images, 50MB for videos)
            var maxSize = type == "Image" ? 10 * 1024 * 1024 : 50 * 1024 * 1024;
            if (file.Length > maxSize)
            {
                TempData["Error"] = $"File size exceeds maximum allowed size.";
                return RedirectToAction("Media");
            }

            var url = await SaveFileAsync(file);

            var media = new GymMedia
            {
                GymId = gymId,
                Url = url,
                Type = type
            };

            _db.GymMedias.Add(media);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"{type} uploaded successfully!";
            return RedirectToAction("Media");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMedia(int id)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var media = await _db.GymMedias
                .Include(m => m.Gym)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (media == null || media.Gym.OwnerId != ownerId)
            {
                return NotFound();
            }

            // Delete physical file
            var filePath = Path.Combine(_environment.WebRootPath, media.Url.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _db.GymMedias.Remove(media);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Media deleted successfully!";
            return RedirectToAction("Media");
        }

        // ===================== REVENUE REPORTS =====================
        [HttpGet]
        public async Task<IActionResult> Revenue()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gym = await _db.Gyms
                .Include(g => g.Bookings)
                .FirstOrDefaultAsync(g => g.OwnerId == ownerId);

            if (gym == null)
            {
                return NotFound();
            }

            var paidBookings = gym.Bookings.Where(b => b.IsPaid && b.IsConfirmedByOwner).ToList();

            var totalRevenue = paidBookings.Sum(b => b.Amount);
            var dailyRevenue = paidBookings.Where(b => b.Type == BookingType.ByDay).Sum(b => b.Amount);
            var monthlyRevenue = paidBookings.Where(b => b.Type == BookingType.ByMonth).Sum(b => b.Amount);
            var trainerRevenue = paidBookings.Where(b => b.Type == BookingType.WithTrainer).Sum(b => b.Amount);

            // Monthly revenue chart (last 6 months)
            var monthlyChart = new Dictionary<string, decimal>();
            for (int i = 5; i >= 0; i--)
            {
                var month = DateTime.UtcNow.AddMonths(-i);
                var revenue = paidBookings
                    .Where(b => b.CreatedAt.Year == month.Year && b.CreatedAt.Month == month.Month)
                    .Sum(b => b.Amount);
                monthlyChart.Add(month.ToString("MMM yyyy"), revenue);
            }

            // Booking type distribution
            var typeDistribution = new Dictionary<BookingType, int>
            {
                { BookingType.ByDay, gym.Bookings.Count(b => b.Type == BookingType.ByDay) },
                { BookingType.ByMonth, gym.Bookings.Count(b => b.Type == BookingType.ByMonth) },
                { BookingType.WithTrainer, gym.Bookings.Count(b => b.Type == BookingType.WithTrainer) }
            };

            var model = new GymRevenueReportViewModel
            {
                Gym = gym,
                TotalRevenue = totalRevenue,
                DailyBookingsRevenue = dailyRevenue,
                MonthlyBookingsRevenue = monthlyRevenue,
                TrainerBookingsRevenue = trainerRevenue,
                TotalPaidBookings = paidBookings.Count,
                TotalUnpaidBookings = gym.Bookings.Count(b => !b.IsPaid),
                MonthlyRevenueChart = monthlyChart,
                BookingTypeDistribution = typeDistribution
            };

            return View(model);
        }

        // ===================== HELPER METHODS =====================
        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            // Create uploads directory if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/" + uniqueFileName;
        }
    }
}
