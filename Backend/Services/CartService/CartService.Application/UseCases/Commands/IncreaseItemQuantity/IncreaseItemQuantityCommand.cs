using MediatR;

namespace CartService.Application.UseCases.Commands.IncreaseItemQuantity
{
    public record IncreaseItemQuantityCommand(string UserId, Guid MealId): IRequest;
}
