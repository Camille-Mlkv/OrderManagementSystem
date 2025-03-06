using MealService.Application.DTOs.Cuisines;
using MediatR;

namespace MealService.Application.UseCases.Cuisines.Commands.AddCuisine
{
    public record AddCuisineCommand(CuisineRequestDto Cuisine) : IRequest<CuisineDto>;
}
