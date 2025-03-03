using MealService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MealService.Infrastructure.Data.Configuration
{
    public class MealEntityConfiguration : BaseEntityConfiguration<Meal>
    {
        protected override void Configure(EntityTypeBuilder<Meal> builder)
        {
            builder.HasOne<Category>()
                .WithMany(c => c.Meals)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Cuisine>()
                .WithMany(c => c.Meals)
                .HasForeignKey(m => m.CuisineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(m => m.Name)
                .IsUnique();

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.Price)
                .IsRequired();
            builder.ToTable(t => t.HasCheckConstraint("CK_Meal_Price", "Price > 0"));

            builder.Property(m => m.Calories)
                .IsRequired();
            builder.ToTable(t => t.HasCheckConstraint("CK_Meal_Calories", "Calories > 0"));

            builder.Property(m => m.IsAvailable)
                .IsRequired();

            builder.Property(m=>m.ImageUrl)
                .IsRequired();

        }
    }
}
