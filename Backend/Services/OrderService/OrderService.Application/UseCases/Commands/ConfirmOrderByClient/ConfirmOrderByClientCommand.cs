using MediatR;

namespace OrderService.Application.UseCases.Commands.ConfirmOrderByClient
{
    public record ConfirmOrderByClientCommand(Guid OrderId): IRequest;
}
