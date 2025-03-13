using MediatR;
using OrderService.Application.DTOs.Address;

namespace OrderService.Application.UseCases.Commands.CreateOrder
{
    public record CreateOrderCommand(Guid ClientId, AddressDto Address) : IRequest<Guid>;
}
