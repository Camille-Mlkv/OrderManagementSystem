using MediatR;

namespace OrderService.Application.UseCases.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(Guid ClientId, string Address) : IRequest<Guid>;
}
