using MediatR;

namespace CartService.Application.UseCases.Commands.DeleteItemFromCart
{
    public record DeleteItemFromCartCommand(Guid UserId, Guid MealId) : IRequest;
}
