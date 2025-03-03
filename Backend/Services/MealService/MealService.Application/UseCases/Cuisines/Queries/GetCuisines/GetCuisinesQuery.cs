using MealService.Application.DTOs.Cuisines;
using MediatR;

namespace MealService.Application.UseCases.Cuisines.Queries.GetCuisines
{
    public record GetCuisinesQuery : IRequest<List<CuisineDto>>
    {
    }
}
