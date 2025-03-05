using Microsoft.EntityFrameworkCore;
using MealService.Domain.Entities;
using MealService.Infrastructure.Data.Configuration;

namespace MealService.Infrastructure.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Meal> Meals { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<MealTag> MealTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MealEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CuisineConfiguration());
            modelBuilder.ApplyConfiguration(new MealTagConfiguration());
        }
    }
}
