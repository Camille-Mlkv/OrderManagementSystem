using MediatR;
using OrderService.Application.DTOs.Order;

namespace OrderService.Application.UseCases.Orders.Queries.GetDeliveredOrdersByDate
{
    public record GetDeliveredOrdersByDateQuery(DateTime From, DateTime To): IRequest<List<OrderDto>>;
}
