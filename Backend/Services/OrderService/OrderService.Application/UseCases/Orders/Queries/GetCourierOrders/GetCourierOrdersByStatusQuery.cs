using MediatR;
using OrderService.Application.DTOs.Order;

namespace OrderService.Application.UseCases.Orders.Queries.GetCourierOrders
{
    public record GetCourierOrdersByStatusQuery(Guid CourierId, string Status): IRequest<List<OrderDto>>;
}
