using MealService.Application.DTOs.Meals;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMeals
{
    public record GetMealsQuery:IRequest<List<MealDto>>
    {
    }
}
