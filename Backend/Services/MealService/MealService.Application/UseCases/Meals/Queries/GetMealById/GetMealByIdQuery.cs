using MealService.Application.DTOs.Meals;
using MediatR;

namespace MealService.Application.UseCases.Meals.Queries.GetMealById
{
    public record GetMealByIdQuery(Guid MealId): IRequest<MealDto>
    {
    }
}
