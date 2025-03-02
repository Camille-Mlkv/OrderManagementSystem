using MealService.Application.DTOs;
using MediatR;

namespace MealService.Application.UseCases.Meals.Commands.CreateMeal
{
    public record CreateMealCommand(MealRequestDto Meal):IRequest<MealDto>
    {
    }
}
