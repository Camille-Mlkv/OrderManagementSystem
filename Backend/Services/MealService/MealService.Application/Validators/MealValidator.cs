using FluentValidation;
using MealService.Application.DTOs.Meals;
using Microsoft.AspNetCore.Http;
using System.Net.NetworkInformation;

namespace MealService.Application.Validators
{
    public class MealValidator:AbstractValidator<MealRequestDto>
    {
        public MealValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("Meal name is required.")
                .MaximumLength(150).WithMessage("Meal name must not exceed 150 characters.");

            RuleFor(m => m.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(m => m.Calories)
                .GreaterThan(0).WithMessage("Calories must be greater than 0.");

            RuleFor(m => m.CategoryId)
                .NotEmpty().WithMessage("Category is required.");

            RuleFor(x => x.ImageContentType)
                .Must(BeAValidImageContentType).WithMessage("Invalid image content type.")
                .When(x => x.ImageData != null);
        }

        private bool BeAValidImageContentType(string? contentType)
        {
            if (contentType is null)
            {
                return true;
            }

            var validImageTypes = new[] { "image/jpeg", "image/png", "image/jpg" };

            return validImageTypes.Contains(contentType.ToLower());
        }
    }
}
