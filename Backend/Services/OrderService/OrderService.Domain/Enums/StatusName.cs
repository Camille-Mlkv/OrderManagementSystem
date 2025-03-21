namespace OrderService.Domain.Enums
{
    public enum StatusName
    {
        // Order is not payed yet
        Pending,

        // Order is paid, preparation starts (notification is sent to admin that order #NUMBER is paid)
        InProgress,

        // Set by admin, waiting for courier to respond
        ReadyForDelivery,

        // Courier is on the way
        OutForDelivery,

        // Status is set when both client and courier confirmed delivery
        Delivered
    }
}
