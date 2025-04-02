namespace OrderService.Infrastructure.Options
{
    public class StripeOptions
    {
        public string SecretKey { get; set; }
        public string WebhookKey { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
