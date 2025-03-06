using MealService.Application.DTOs.Meals;
using MediatR;

namespace MealService.Application.UseCases.Meals.Commands.AddMeal
{
    public record AddMealCommand(MealRequestDto Meal) : IRequest<MealDto>;
}
