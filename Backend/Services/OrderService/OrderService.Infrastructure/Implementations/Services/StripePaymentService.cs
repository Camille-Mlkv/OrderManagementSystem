using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderService.Application.DTOs.Payment;
using OrderService.Application.Specifications.Services;
using Stripe;
using Stripe.Checkout;

namespace OrderService.Infrastructure.Implementations.Services
{
    public class StripePaymentService: IPaymentService
    {
        private readonly StripeSettings _stripeSettings;

        public StripePaymentService(IOptions<StripeSettings> stripeSettings, ILogger<StripePaymentService> logger)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        public async Task<PaymentResult> CreateCheckoutSessionAsync(Guid orderId, decimal amount, string clientEmail)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(amount * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Order {orderId}"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = _stripeSettings.SuccessUrl,
                CancelUrl = _stripeSettings.CancelUrl,
                CustomerEmail = clientEmail,
                Metadata = new Dictionary<string, string>
                {
                    { "orderId", orderId.ToString() }
                }
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return new PaymentResult
            {
                Success = true,
                SessionId = session.Id,
                PaymentUrl = session.Url
            };
        }

        public Guid? HandleWebhook(string json, string stripeSignature)
        {
            var endpointSecret = _stripeSettings.WebhookKey;
            Event stripeEvent =  EventUtility.ConstructEvent(json, stripeSignature, endpointSecret,300,false);

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                var orderId = Guid.Parse(session?.Metadata["orderId"]!);

                return orderId;
            }

            return null;
        }
    }
}
