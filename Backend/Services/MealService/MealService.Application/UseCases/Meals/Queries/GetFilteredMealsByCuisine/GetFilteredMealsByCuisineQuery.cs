using MealService.Application.DTOs;
using MealService.Application.DTOs.Meals;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetFilteredMealsByCuisine
{
    public record GetFilteredMealsByCuisineQuery(Guid CuisineId, MealFilterDto Filter, int PageNo, int PageSize) : IRequest<PagedList<MealDto>>;
}
