using MealService.Application.DTOs.Cuisines;
using MediatR;

namespace MealService.Application.UseCases.Cuisines.Commands.UpdateCuisine
{
    public record UpdateCuisineCommand(Guid Id, CuisineRequestDto Cuisine) : IRequest<CuisineDto>;
}
