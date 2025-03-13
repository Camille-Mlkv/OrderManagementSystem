using MediatR;

namespace MealService.Application.UseCases.Tags.Commands.DeleteTag
{
    public record DeleteTagCommand(Guid Id) : IRequest<Unit>;
}
