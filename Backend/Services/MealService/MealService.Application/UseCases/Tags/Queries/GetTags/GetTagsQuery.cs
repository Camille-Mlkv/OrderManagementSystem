using MealService.Application.DTOs.Tags;
using MediatR;

namespace MealService.Application.UseCases.Tags.Queries.GetTags
{
    public record GetTagsQuery : IRequest<List<TagDto>>
    {
    }
}
