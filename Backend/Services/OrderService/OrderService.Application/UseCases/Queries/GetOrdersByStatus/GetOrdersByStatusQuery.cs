using MediatR;
using OrderService.Application.DTOs.Order;
using System.Globalization;

namespace OrderService.Application.UseCases.Queries.GetOrdersByStatus
{
    public record GetOrdersByStatusQuery(string Status): IRequest<List<OrderDto>>;
}
