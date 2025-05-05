using MealService.Application.DTOs.Cuisines;
using MediatR;

namespace MealService.Application.UseCases.Cuisines.Queries.GetCuisineById
{
    public record GetCuisineByIdQuery(Guid Id): IRequest<CuisineDto>;
}
