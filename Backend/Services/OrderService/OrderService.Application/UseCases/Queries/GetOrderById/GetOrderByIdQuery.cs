using MediatR;
using OrderService.Application.DTOs.Order;

namespace OrderService.Application.UseCases.Queries.GetOrderById
{
    public record GetOrderByIdQuery(Guid Id): IRequest<OrderDto>;
}
