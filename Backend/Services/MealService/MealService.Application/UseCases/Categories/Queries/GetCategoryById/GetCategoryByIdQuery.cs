using MealService.Application.DTOs.Categories;
using MediatR;

namespace MealService.Application.UseCases.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(Guid Id): IRequest<CategoryDto>;
}
