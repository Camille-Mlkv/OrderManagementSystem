using FluentValidation;
using MealService.Application.DTOs.Meals;
using Microsoft.AspNetCore.Http;

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

            RuleFor(m => m.ImageFile)
                .Must(BeAValidImage).When(m => m.ImageFile != null)
                .WithMessage("Allowed image formats: .jpg, .jpeg, .png. Maximum size: 5MB.");

            RuleFor(m => m.CategoryId)
                .NotEmpty().WithMessage("Category is required.");
        }

        private bool BeAValidImage(IFormFile? file)
        {
            if (file == null)
                return true;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(fileExtension);
        }
    }
}
