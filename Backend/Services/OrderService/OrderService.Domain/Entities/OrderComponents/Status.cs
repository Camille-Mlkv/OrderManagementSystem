using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities.OrderComponents
{
    public class Status
    {
        public int Id { get; set; }
        public StatusName Name => (StatusName)Id;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
