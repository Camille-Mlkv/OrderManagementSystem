using MediatR;
using OrderService.Application.DTOs.Order;

namespace OrderService.Application.UseCases.Queries.GetCourierOrders
{
    public record GetCourierOrdersQuery(Guid CourierId): IRequest<List<OrderDto>>;
}
