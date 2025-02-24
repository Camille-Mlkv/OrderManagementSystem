using MealService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MealService.Infrastructure.Data.Configuration
{
    public class CategoryEntityConfiguration : BaseEntityConfiguration<Category>
    {
        protected override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(entity => entity.Meals)
                    .WithOne(meal => meal.Category)
                    .HasForeignKey(meal => meal.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(c => c.Name)
                .IsUnique();

        }
    }
}
