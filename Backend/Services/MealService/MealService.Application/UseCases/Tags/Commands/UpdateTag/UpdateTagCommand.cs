using MealService.Application.DTOs.Tags;
using MediatR;

namespace MealService.Application.UseCases.Tags.Commands.UpdateTag
{
    public record UpdateTagCommand(Guid TagId, TagRequestDto Tag): IRequest<TagDto>
    {
    }
}
