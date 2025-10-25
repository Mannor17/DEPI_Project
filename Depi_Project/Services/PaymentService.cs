namespace Depi_Project.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<(bool Success, string PaymentUrl)> CreatePaymentIntentAsync(decimal amount, int bookingId)
        {
            // هنا تقدر تربطي Paypal/Stripe .. الآن نرجع URL وهمي
            return Task.FromResult((true, $"/payments/mockpay?bookingId={bookingId}"));
        }
        public Task<bool> VerifyPaymentAsync(string transactionId)
        {
            // تحقق من الموفر الحقيقي في تنفيذ حقيقي
            return Task.FromResult(true);
        }
    }
}
