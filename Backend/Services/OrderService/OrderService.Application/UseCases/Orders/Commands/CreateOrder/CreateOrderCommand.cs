using MediatR;
using OrderService.Application.DTOs.Address;

namespace OrderService.Application.UseCases.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(Guid ClientId, AddressDto Address) : IRequest<Guid>;
}
