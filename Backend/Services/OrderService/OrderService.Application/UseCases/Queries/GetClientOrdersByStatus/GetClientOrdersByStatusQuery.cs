using MediatR;
using OrderService.Application.DTOs.Order;

namespace OrderService.Application.UseCases.Queries.GetClientOrdersByStatus
{
    public record GetClientOrdersByStatusQuery(Guid ClientId, string Status): IRequest<List<OrderDto>>;
}
