using MediatR;

namespace OrderService.Application.UseCases.Orders.Commands.UpdateOrderWithCourierId
{
    public record UpdateOrderWithCourierIdCommand(Guid CourierId, Guid OrderId) : IRequest;
}
