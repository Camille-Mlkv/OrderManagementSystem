using OrderService.Application.DTOs.Payment;

namespace OrderService.Application.Specifications.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> CreateCheckoutSessionAsync(Guid orderId, decimal amount, string clientEmail);
        Guid? HandleWebhook(string json, string stripeSignature);
    }
}
