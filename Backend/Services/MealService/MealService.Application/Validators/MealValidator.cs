using FluentValidation;
using MealService.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace MealService.Application.Validators
{
    public class MealValidator:AbstractValidator<MealRequestDto>
    {
        public MealValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("Meal name is required.")
                .MaximumLength(100).WithMessage("Meal name must not exceed 100 characters.");

            RuleFor(m => m.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

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
