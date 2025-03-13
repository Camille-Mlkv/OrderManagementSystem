using FluentValidation;
using MealService.Application.DTOs.Cuisines;

namespace MealService.Application.Validators
{
    public class CuisineValidator: AbstractValidator<CuisineRequestDto>
    {
        public CuisineValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Cuisine name is required.")
                .MaximumLength(100).WithMessage("Name can't exceed 100 characters.");
        }
    }
}
