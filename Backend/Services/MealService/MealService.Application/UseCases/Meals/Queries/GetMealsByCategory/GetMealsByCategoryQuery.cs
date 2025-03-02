using MealService.Application.DTOs;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMealsByCategory
{
    public record GetMealsByCategoryQuery(Guid CategoryId, int PageNo, int PageSize) : IRequest<List<MealDto>>
    {
    }
}
