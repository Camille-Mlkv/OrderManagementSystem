using MediatR;

namespace OrderService.Application.UseCases.Commands.DeletePendingOrder
{
    public record DeletePendingOrderCommand(Guid OrderId) : IRequest;
}
