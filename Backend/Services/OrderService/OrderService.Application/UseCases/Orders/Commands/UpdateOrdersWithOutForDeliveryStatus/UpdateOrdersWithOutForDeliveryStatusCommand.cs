using MediatR;

namespace OrderService.Application.UseCases.Orders.Commands.UpdateOrdersWithOutForDeliveryStatus
{
    public record UpdateOrdersWithOutForDeliveryStatusCommand(Guid CourierId): IRequest;
}
