using MealService.Application.DTOs.Tags;
using MediatR;

namespace MealService.Application.UseCases.Tags.Commands.AddTag
{
    public record AddTagCommand(TagRequestDto Tag): IRequest<TagDto>
    {
    }
}
