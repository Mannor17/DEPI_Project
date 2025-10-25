using System.Collections.Generic;

namespace Depi_Project.Models.ViewModels
{
    public class ProfileViewModel
    {
        public ApplicationUser User { get; set; }      // بيانات المستخدم
        public List<Booking> Bookings { get; set; }
    }
}
