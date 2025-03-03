using MealService.Application.DTOs.Meals;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetFilteredMeals
{
    public record GetFilteredMealsQuery(MealFilterDto Filter, int PageNo, int PageSize): IRequest<List<MealDto>>
    {
    }
}
