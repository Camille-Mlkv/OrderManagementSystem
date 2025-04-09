using MealService.Application.DTOs.Meals;
using MediatR;
using MealService.Application.DTOs;

namespace MealService.Application.UseCases.Meals.Queries.GetMealsByCuisine
{
    public record GetMealsByCuisineQuery(Guid CuisineId,int PageNo, int PageSize, bool? IsAvailable) : IRequest<PagedList<MealDto>>;
}
