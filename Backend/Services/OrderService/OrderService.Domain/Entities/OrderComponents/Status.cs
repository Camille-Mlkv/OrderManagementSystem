using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities.OrderComponents
{
    public class Status
    {
        public StatusName Name;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
