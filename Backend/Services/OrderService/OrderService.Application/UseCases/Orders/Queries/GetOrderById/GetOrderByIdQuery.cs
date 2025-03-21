using MediatR;
using OrderService.Application.DTOs.Order;

namespace OrderService.Application.UseCases.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery(Guid Id): IRequest<OrderDto>;
}
