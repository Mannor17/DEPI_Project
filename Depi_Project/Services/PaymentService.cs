using Stripe;
using Depi_Project.Models;

namespace Depi_Project.Services
{
    public class PaymentService : IPaymentService
    {
		public async Task<PaymentResult> CreatePaymentIntentAsync(decimal amount, int bookingId)
		{
			var options = new PaymentIntentCreateOptions
			{
				Amount = (long)(amount * 100), // بالـ cents
				Currency = "usd",
				PaymentMethodTypes = new List<string> { "card" }, // يدعم البطاقة
				Metadata = new Dictionary<string, string>
			{
				{ "BookingId", bookingId.ToString() }
			}
			};

			var service = new PaymentIntentService();
			var paymentIntent = await service.CreateAsync(options);

			return new PaymentResult
			{
				PaymentIntentId = paymentIntent.Id,
				ClientSecret = paymentIntent.ClientSecret,
				PaymentUrl = "#" // مش محتاجه دلوقتي لأننا هنستخدم JS
			};
		}

		public async Task<bool> VerifyPaymentAsync(string paymentIntentId)
		{
			var service = new PaymentIntentService();
			var intent = await service.GetAsync(paymentIntentId);
			return intent.Status == "succeeded";
		}
	}
}
