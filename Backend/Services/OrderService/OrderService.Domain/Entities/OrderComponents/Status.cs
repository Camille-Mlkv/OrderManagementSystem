using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities.OrderComponents
{
    public class Status
    {
        private StatusName _name;

        public StatusName Name
        {
            get => _name;
            set
            {
                _name = value;
                UpdatedAt = DateTime.UtcNow;
            }
        }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
