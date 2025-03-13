using MealService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MealService.Infrastructure.Data.Configuration
{
    public class MealTagConfiguration : BaseEntityConfiguration<MealTag>
    {
        protected override void Configure(EntityTypeBuilder<MealTag> builder)
        {
            builder.HasKey(mt => new { mt.MealId, mt.TagId });

            builder.HasOne(mt => mt.Meal)
                .WithMany(m => m.MealTags)
                .HasForeignKey(mt => mt.MealId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(mt => mt.Tag)
                .WithMany(t => t.MealTags)
                .HasForeignKey(mt => mt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
