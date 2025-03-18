using MediatR;

namespace OrderService.Application.UseCases.Orders.Commands.ConfirmOrderByClient
{
    public record ConfirmOrderByClientCommand(Guid OrderId): IRequest;
}
