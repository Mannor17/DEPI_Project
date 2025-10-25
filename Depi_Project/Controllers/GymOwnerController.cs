using Depi_Project.Data;
using Depi_Project.Models;
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
        public GymOwnerController(ApplicationDbContext db) { _db = db; }

        public async Task<IActionResult> Dashboard()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gym = await _db.Gyms.Include(g => g.Bookings)
                                    .FirstOrDefaultAsync(g => g.OwnerId == ownerId);
            return View(gym);
        }

        public IActionResult Bookings() => View();
        public IActionResult Media() => View();

        public async Task<IActionResult> ApproveBooking(int id)
        {
            var booking = await _db.Bookings.Include(b => b.Gym)
                                            .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return NotFound();
            if (booking.Gym.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Forbid();

            booking.IsConfirmedByOwner = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> RejectBooking(int id)
        {
            var booking = await _db.Bookings.Include(b => b.Gym)
                                            .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return NotFound();
            if (booking.Gym.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Forbid();

            booking.IsCancelled = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> UploadMedia(int gymId, IFormFile file, string type)
        {
            var url = await SaveFileAsync(file);
            _db.GymMedias.Add(new GymMedia { GymId = gymId, Url = url, Type = type });
            await _db.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "/uploads/" + fileName;
        }
    }
}
