using MealService.Application.DTOs;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMeals
{
    public record GetMealsQuery:IRequest<List<MealDto>>
    {
    }
}
