using Microsoft.AspNetCore.Mvc;

namespace Depi_Project.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Profile()
		{
			return View();
		}
		public IActionResult Booking() 
		{
			return View();
		}
	}
}
