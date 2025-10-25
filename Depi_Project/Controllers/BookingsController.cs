using Depi_Project.Data;
using Depi_Project.Models;
using Depi_Project.Models.ViewModels;
using Depi_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;



namespace Depi_Project.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPaymentService _paymentService;

        public BookingsController(ApplicationDbContext db, IPaymentService paymentService)
        {
            _db = db;
            _paymentService = paymentService;
        }

        public IActionResult Create(int gymId, BookingType type)
        {
            var vm = new CreateBookingViewModel
            {
                GymId = gymId,
                Type = type
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookingViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var booking = new Booking
            {
                GymId = vm.GymId,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Type = vm.Type,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                TrainerId = vm.TrainerId,
                Amount = vm.Amount
            };

            // ✅ التحقق من وجود حجز مشابه لنفس المستخدم والتاريخ
            var exists = await _db.Bookings.AnyAsync(b =>
                b.UserId == booking.UserId &&
                b.GymId == booking.GymId &&
                b.StartDate == booking.StartDate &&
                !b.IsCancelled);

            if (exists)
            {
                ModelState.AddModelError("", "لديك حجز مشابه بالفعل في نفس التاريخ.");
                return View(vm);
            }

            await _db.Bookings.AddAsync(booking);
            await _db.SaveChangesAsync();

            // بعد إنشاء الحجز، الانتقال لصفحة الدفع
            return RedirectToAction(nameof(Pay), new { id = booking.Id });
        }

        public async Task<IActionResult> Pay(int id)
        {
            var booking = await _db.Bookings
                .Include(b => b.Gym)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            if (booking.IsPaid)
                return RedirectToAction(nameof(Details), new { id });

            // إنشاء عملية الدفع
            var paymentResult = await _paymentService.CreatePaymentIntentAsync(booking.Amount, booking.Id);

            return View(new PaymentViewModel
            {
                Booking = booking,
                PaymentUrl = paymentResult.PaymentUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int id, string transactionId)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            // التحقق من الدفع
            var ok = await _paymentService.VerifyPaymentAsync(transactionId);
            if (!ok)
            {
                TempData["Error"] = "عملية الدفع لم تكتمل بنجاح.";
                return RedirectToAction(nameof(Pay), new { id });
            }

            booking.IsPaid = true;
            booking.PaymentReceiptUrl = $"receipts/{transactionId}";
            await _db.SaveChangesAsync();

            // بعد الدفع بنجاح
            return RedirectToAction(nameof(Details), new { id = booking.Id });
        }

        // ✅ إضافة صفحة تفاصيل الحجز لتجنب الخطأ CS0103
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _db.Bookings
                .Include(b => b.Gym)
                .Include(b => b.Trainer)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }
    }
}

