using MealService.Application.DTOs.Meals;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMealsPerPage
{
    public record GetMealsPerPageQuery(int PageNo, int PageSize):IRequest<List<MealDto>>
    {
    }
}
