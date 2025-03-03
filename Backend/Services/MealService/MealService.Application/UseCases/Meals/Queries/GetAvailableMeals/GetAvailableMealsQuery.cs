using MealService.Application.DTOs.Meals;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetAvailableMeals
{
    public record GetAvailableMealsQuery(int PageNo, int PageSize) :IRequest<List<MealDto>>
    {
    }
}
