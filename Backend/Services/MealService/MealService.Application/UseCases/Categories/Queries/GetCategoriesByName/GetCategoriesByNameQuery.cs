using MealService.Application.DTOs.Categories;
using MediatR;

namespace MealService.Application.UseCases.Categories.Queries.GetCategoriesByName
{
    public record GetCategoriesByNameQuery(string Name):IRequest<List<CategoryDto>>
    {
    }
}
