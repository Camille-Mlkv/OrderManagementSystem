namespace OrderService.Application.Utilities
{
    public class OrderNumberGenerator
    {
        private static readonly Random _random = new Random();

        public string GenerateOrderNumber()
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var randomPart = new Random().Next(100000, 999999);

            return $"ORD-{datePart}-{randomPart}";
        }
    }
}
