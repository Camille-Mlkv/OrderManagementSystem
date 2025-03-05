using MealService.Application.DTOs.Meals;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMealsByName
{
    public record GetMealsByNameQuery(string Name): IRequest<List<MealDto>>
    {
    }
}
