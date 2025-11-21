using Depi_Project.Data;
using Depi_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Depi_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db) { _db = db; }


        public async Task<IActionResult> Index(string q, double? lat, double? lng)
        {
            var gyms = _db.Gyms.Include(g => g.Reviews).Include(g => g.Media).AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
                gyms = gyms.Where(g => g.Name.Contains(q) || g.Description.Contains(q));

            if (lat.HasValue && lng.HasValue)
            {
                gyms = gyms.OrderBy(g => (g.Latitude - lat.Value) * (g.Latitude - lat.Value) +
                                         (g.Longitude - lng.Value) * (g.Longitude - lng.Value));
            }

            var list = await gyms.Take(50).ToListAsync();
            return View(list);
        }

        //public async Task<IActionResult> Details(int id)
        //{
        //    var gym = await _db.Gyms
        //        .Include(g => g.Trainers)
        //        .Include(g => g.Reviews).ThenInclude(r => r.User)
        //        .Include(g => g.Media)
        //        .FirstOrDefaultAsync(g => g.Id == id);

        //    if (gym == null) return NotFound();
        //    return View(gym);
        //}
    }
}

