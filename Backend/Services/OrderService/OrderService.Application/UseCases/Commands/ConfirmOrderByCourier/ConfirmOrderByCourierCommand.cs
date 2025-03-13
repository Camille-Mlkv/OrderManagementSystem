using MediatR;

namespace OrderService.Application.UseCases.Commands.ConfirmOrderByCourier
{
    public record ConfirmOrderByCourierCommand(Guid OrderId): IRequest;
}
