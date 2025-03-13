using MediatR;

namespace OrderService.Application.UseCases.Commands.UpdateOrderWithCourierId
{
    public record UpdateOrderWithCourierIdCommand(Guid CourierId, Guid OrderId) : IRequest;
}
