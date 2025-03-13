using MediatR;

namespace MealService.Application.UseCases.Meals.Commands.DeleteMeal
{
    public record DeleteMealCommand(Guid MealId) : IRequest<Unit>;
}
