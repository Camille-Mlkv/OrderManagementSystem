using MediatR;

namespace MealService.Application.UseCases.Cuisines.Commands.DeleteCuisine
{
    public record DeleteCuisineCommand(Guid Id): IRequest<Unit>
    {
    }
}
