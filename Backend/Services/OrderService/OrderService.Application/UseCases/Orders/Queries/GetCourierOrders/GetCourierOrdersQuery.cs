using MediatR;
using OrderService.Application.DTOs.Order;

namespace OrderService.Application.UseCases.Orders.Queries.GetCourierOrders
{
    public record GetCourierOrdersQuery(Guid CourierId): IRequest<List<OrderDto>>;
}
