namespace OrderService.Application.DTOs.Payment
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentUrl { get; set; }
    }
}
