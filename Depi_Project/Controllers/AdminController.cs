using Depi_Project.Data;
using Depi_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Depi_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager; _db = db;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Accounts()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> ToggleAccount(string id, bool enable)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            user.LockoutEnd = enable ? null : DateTimeOffset.MaxValue;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> DeleteAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Users");
        }
    }
}
