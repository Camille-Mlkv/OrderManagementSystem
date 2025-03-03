using MealService.Application.DTOs.Meals;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetAllMeals
{
    public record GetAllMealsQuery:IRequest<List<MealDto>>
    {
    }
}
