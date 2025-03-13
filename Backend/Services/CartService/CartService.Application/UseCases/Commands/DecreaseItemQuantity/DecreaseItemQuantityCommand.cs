using MediatR;

namespace CartService.Application.UseCases.Commands.DecreaseItemQuantity
{
    public record DecreaseItemQuantityCommand(string UserId, Guid MealId) : IRequest;
}
