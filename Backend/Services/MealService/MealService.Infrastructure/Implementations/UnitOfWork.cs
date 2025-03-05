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
        private readonly IRepository<Cuisine> _cuisineRepository;
        private readonly IRepository<Tag> _tagRepository;

        public UnitOfWork(AppDbContext context, 
            IRepository<Meal> mealRepository, 
            IRepository<Category> categoryRepository, 
            IRepository<Cuisine> cuisineRepository, 
            IRepository<Tag> tagRepository)
        {
            _context = context;
            _mealRepository = mealRepository;
            _categoryRepository = categoryRepository;
            _cuisineRepository = cuisineRepository;
            _tagRepository = tagRepository;
        }
        public IRepository<Meal> MealRepository => _mealRepository;

        public IRepository<Category> CategoryRepository => _categoryRepository;

        public IRepository<Cuisine> CuisineRepository => _cuisineRepository;

        public IRepository<Tag> TagRepository => _tagRepository;

        public async Task SaveAllAsync(CancellationToken cancellationToken) 
            => await _context.SaveChangesAsync(cancellationToken);
    }
}
