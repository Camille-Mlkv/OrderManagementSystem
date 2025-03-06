using MealService.Application.DTOs.Categories;
using MediatR;

namespace MealService.Application.UseCases.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery : IRequest<List<CategoryDto>>;
}
