namespace Depi_Project.Models.ViewModels
{
    public class PaymentViewModel
    {
        public Booking Booking { get; set; }
		public string PaymentIntentId { get; set; }
		public string ClientSecret { get; set; }// تفاصيل الحجز نفسه
		public string PaymentUrl { get; set; }         // رابط الدفع اللي جاي من PaymentService
        public string TransactionId { get; set; }      // لو عايزة تحفظ رقم العملية
        public string PublishableKey {  get; set; }

		public bool PaymentSuccess { get; set; }
    }
}
