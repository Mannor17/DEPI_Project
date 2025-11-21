using Depi_Project.Data;
using Depi_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Depi_Project.Controllers
{
	[Authorize]
	public class FavoritesController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public FavoritesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		[HttpPost]
		public async Task<IActionResult> Add(int gymId)
		{
			var user = await _userManager.GetUserAsync(User);

			// Check if already exists
			var exists = _context.Favorites
				.Any(f => f.GymId == gymId && f.UserId == user.Id);

			if (!exists)
			{
				var fav = new Favorite
				{
					GymId = gymId,
					UserId = user.Id
				};

				_context.Favorites.Add(fav);
				await _context.SaveChangesAsync();
			}

			return Redirect($"/User/Details/{gymId}");
		}
	}
}
