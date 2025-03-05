using FluentValidation;
using MealService.Application.DTOs.Tags;

namespace MealService.Application.Validators
{
    public class TagValidator:AbstractValidator<TagRequestDto>
    {
        public TagValidator()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("Tag name is required.")
                .MaximumLength(20).WithMessage("Tag name must not exceed 20 characters.");

            RuleFor(t => t.Description)
                .NotEmpty().WithMessage("Tag description is required.")
                .MaximumLength(100).WithMessage("Tag description must not exceed 100 characters.");
        }
    }
}
