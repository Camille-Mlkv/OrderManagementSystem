using MediatR;

namespace CartService.Application.UseCases.Commands.DeleteItemFromCart
{
    public record DeleteItemFromCartCommand(string UserId, Guid MealId) : IRequest;
}
