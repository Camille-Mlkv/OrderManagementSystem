using MealService.Application.Specifications.Repositories;
using MealService.Domain.Entities;

namespace MealService.Application.Specifications
{
    public interface IUnitOfWork
    {
        IRepository<Meal> MealRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<Cuisine> CuisineRepository { get; }
        public Task SaveAllAsync(CancellationToken cancellationToken);
    }
}
