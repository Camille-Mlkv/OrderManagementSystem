using MediatR;

namespace CartService.Application.UseCases.Commands.IncreaseItemQuantity
{
    public record IncreaseItemQuantityCommand(Guid UserId, Guid MealId): IRequest;
}
