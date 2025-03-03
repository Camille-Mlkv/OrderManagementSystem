using MealService.Application.DTOs;
using MealService.Application.DTOs.Categories;
using MediatR;

namespace MealService.Application.UseCases.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(Guid Id, CategoryRequestDto Category) :IRequest<CategoryDto>
    {
    }
}
