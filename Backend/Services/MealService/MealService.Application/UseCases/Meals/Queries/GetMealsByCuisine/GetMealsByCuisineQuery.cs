using MealService.Application.DTOs.Meals;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMealsByCuisine
{
    public record GetMealsByCuisineQuery(Guid CuisineId,int PageNo, int PageSize, bool? IsAvailable) : IRequest<List<MealDto>>;
}
