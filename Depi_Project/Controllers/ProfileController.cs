using Depi_Project.Data;
using Depi_Project.Models;
using Depi_Project.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Depi_Project.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfileController(ApplicationDbContext db, UserManager<ApplicationUser> userManager) { _db = db; _userManager = userManager; }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var bookings = await _db.Bookings.Where(b => b.UserId == userId).Include(b => b.Gym).ToListAsync();
            var vm = new ProfileViewModel { User = user, Bookings = bookings };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel vm)
        {
            var user = await _userManager.FindByIdAsync(vm.User.Id);
            if (user == null) return NotFound();
            user.FullName = vm.User.FullName;
            user.IsProfilePublic = vm.User.IsProfilePublic;
            // other updates...
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
    }
}
