using MediatR;

namespace OrderService.Application.UseCases.Orders.Commands.DeletePendingOrder
{
    public record DeletePendingOrderCommand(Guid OrderId) : IRequest;
}
