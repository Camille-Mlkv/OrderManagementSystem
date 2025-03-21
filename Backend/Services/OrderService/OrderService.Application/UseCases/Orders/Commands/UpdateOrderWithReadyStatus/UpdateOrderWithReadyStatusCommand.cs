using MediatR;

namespace OrderService.Application.UseCases.Orders.Commands.UpdateOrderWithReadyStatus
{
    public record UpdateOrderWithReadyStatusCommand(Guid OrderId): IRequest;

}
