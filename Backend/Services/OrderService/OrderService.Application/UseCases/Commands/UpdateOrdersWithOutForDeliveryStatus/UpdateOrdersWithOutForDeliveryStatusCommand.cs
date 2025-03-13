using MediatR;

namespace OrderService.Application.UseCases.Commands.UpdateOrdersWithOutForDeliveryStatus
{
    public record UpdateOrdersWithOutForDeliveryStatusCommand(Guid CourierId): IRequest;
}
