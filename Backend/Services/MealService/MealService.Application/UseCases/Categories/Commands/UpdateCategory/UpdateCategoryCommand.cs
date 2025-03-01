using MealService.Application.DTOs;
using MediatR;

namespace MealService.Application.UseCases.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(Guid Id, UpdateCategoryDto Category) :IRequest<CategoryDto>
    {
    }
}
