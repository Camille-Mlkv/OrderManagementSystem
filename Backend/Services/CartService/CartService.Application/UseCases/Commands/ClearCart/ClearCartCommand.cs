using MediatR;

namespace CartService.Application.UseCases.Commands.ClearCart
{
    public record ClearCartCommand(Guid UserId): IRequest;
}
