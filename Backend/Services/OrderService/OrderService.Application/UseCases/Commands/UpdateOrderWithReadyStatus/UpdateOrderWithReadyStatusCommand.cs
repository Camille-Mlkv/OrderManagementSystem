using MediatR;

namespace OrderService.Application.UseCases.Commands.UpdateOrderWithReadyStatus
{
    public record UpdateOrderWithReadyStatusCommand(Guid OrderId): IRequest;

}
