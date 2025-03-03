using MealService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MealService.Infrastructure.Data.Configuration
{
    public class CuisineConfiguration : BaseEntityConfiguration<Cuisine>
    {
        protected override void Configure(EntityTypeBuilder<Cuisine> builder)
        {
            builder.HasMany(entity => entity.Meals)
                    .WithOne(meal => meal.Cuisine)
                    .HasForeignKey(meal => meal.CuisineId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.ImageUrl)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
