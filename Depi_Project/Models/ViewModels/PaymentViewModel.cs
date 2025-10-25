namespace Depi_Project.Models.ViewModels
{
    public class PaymentViewModel
    {
        public Booking Booking { get; set; }           // تفاصيل الحجز نفسه
        public string PaymentUrl { get; set; }         // رابط الدفع اللي جاي من PaymentService
        public string TransactionId { get; set; }      // لو عايزة تحفظ رقم العملية
        public bool PaymentSuccess { get; set; }
    }
}
