using MealService.Application.DTOs.Cuisines;
using MediatR;

namespace MealService.Application.UseCases.Cuisines.Queries.GetCuisinesByName
{
    public record GetCuisinesByNameQuery(string Name):IRequest<List<CuisineDto>>
    {
    }
}
