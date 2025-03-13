using MealService.Application.DTOs.Meals;
using MediatR;

namespace MealService.Application.UseCases.Meals.Commands.UpdateMeal
{
    public record UpdateMealCommand(Guid Id, MealRequestDto UpdatedMeal) : IRequest<MealDto>;
}
