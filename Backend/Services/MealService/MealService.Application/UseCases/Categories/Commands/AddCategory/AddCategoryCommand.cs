using MealService.Application.DTOs.Categories;
using MediatR;

namespace MealService.Application.UseCases.Categories.Commands.AddCategory
{
    public record AddCategoryCommand(CategoryRequestDto Category) : IRequest<CategoryDto>;
}
