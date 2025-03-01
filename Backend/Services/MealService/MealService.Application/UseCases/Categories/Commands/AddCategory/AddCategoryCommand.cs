using MealService.Application.DTOs;
using MediatR;

namespace MealService.Application.UseCases.Categories.Commands.AddCategory
{
    public record AddCategoryCommand(CategoryDto Category) : IRequest<CategoryDto>
    {
    }
}
