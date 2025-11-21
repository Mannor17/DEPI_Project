using Depi_Project.Models;

namespace Depi_Project.Services
{
    public interface IPaymentService
    {
        //Task<(bool Success, string PaymentUrl)> CreatePaymentIntentAsync(decimal amount, int bookingId);
        //Task<bool> VerifyPaymentAsync(string transactionId);
		Task<PaymentResult> CreatePaymentIntentAsync(decimal amount, int bookingId);
		Task<bool> VerifyPaymentAsync(string transactionId);
	}
}
