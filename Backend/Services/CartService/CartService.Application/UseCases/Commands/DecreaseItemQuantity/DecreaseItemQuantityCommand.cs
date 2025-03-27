using MediatR;

namespace CartService.Application.UseCases.Commands.DecreaseItemQuantity
{
    public record DecreaseItemQuantityCommand(Guid UserId, Guid MealId) : IRequest;
}
