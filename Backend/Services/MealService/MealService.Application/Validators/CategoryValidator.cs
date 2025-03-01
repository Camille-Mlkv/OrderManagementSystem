using FluentValidation;
using MealService.Application.DTOs;

namespace MealService.Application.Validators
{
    public class CategoryValidator:AbstractValidator<CategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Category is required.")
                .MaximumLength(100).WithMessage("Name can't exceed 100 characters.");

            RuleFor(c => c.NormalizedName)
                .NotEmpty().WithMessage("NormalizedName is required.")
                .Must((dto, normalizedName) => normalizedName == dto.Name.ToUpper())
                .WithMessage("NormalizedName must be similar to Name in uppercase.");
        }
    }
}
