using System.Collections.Generic;
using Depi_Project.Models;

namespace Depi_Project.Models.ViewModels
{
    public class ProfileViewModel
    {
        // استخدمنا Id كـ string لأن IdentityUser.Id هو string
        public string Id { get; set; } = string.Empty;

        // مطابق للي عندك في ApplicationUser
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // إعدادات البروفايل (موجودة في ApplicationUser)
        public bool IsProfilePublic { get; set; } = true;

        // بيانات عرض إضافية
        public int TotalBookings { get; set; } = 0;

        // لو عايزة تعملي قائمة بالحجوزات للعرض في الواجهة
        public List<Booking>? Bookings { get; set; }
    }
}
