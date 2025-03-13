using MediatR;
using OrderService.Application.DTOs.Order;

namespace OrderService.Application.UseCases.Queries.GetOpenedOrders
{
    public record GetOpenedOrdersQuery: IRequest<List<OpenedOrderDto>>;
}
