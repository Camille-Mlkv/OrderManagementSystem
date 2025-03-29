namespace UserService.BusinessLogic.DTOs.Order
{
    public class OrderStatusDto
    {
        public Guid UserId { get; set; }
        public string OrderNumber { get; set; }
        public string OrderStatus { get; set; }
    }
}
