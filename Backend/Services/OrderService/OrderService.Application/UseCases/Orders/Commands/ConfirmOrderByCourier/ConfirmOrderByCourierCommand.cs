using MediatR;

namespace OrderService.Application.UseCases.Orders.Commands.ConfirmOrderByCourier
{
    public record ConfirmOrderByCourierCommand(Guid OrderId): IRequest;
}
