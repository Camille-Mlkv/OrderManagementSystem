using MealService.Application.Specifications;
using MealService.Application.Specifications.Repositories;
using MealService.Domain.Entities;
using MealService.Infrastructure.Data;

namespace MealService.Infrastructure.Implementations
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IRepository<Meal> _mealRepository;
        private readonly IRepository<Category> _categoryRepository;

        public UnitOfWork(AppDbContext context, IRepository<Meal> mealRepository, IRepository<Category> categoryRepository)
        {
            _context = context;
            _mealRepository = mealRepository;
            _categoryRepository = categoryRepository;
        }
        public IRepository<Meal> MealRepository => _mealRepository;
        public IRepository<Category> CategoryRepository => _categoryRepository;
        public async Task SaveAllAsync(CancellationToken cancellationToken) 
            => await _context.SaveChangesAsync(cancellationToken);
    }
}
