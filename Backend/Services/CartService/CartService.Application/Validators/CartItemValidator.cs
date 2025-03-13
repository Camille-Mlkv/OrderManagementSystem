using CartService.Application.DTOs;
using FluentValidation;

namespace CartService.Application.Validators
{
    public class CartItemValidator: AbstractValidator<CartItemDto>
    {
        public CartItemValidator()
        {
            RuleFor(i => i.MealId).NotEmpty()
                .WithMessage("Meal id can't be empty.");

            RuleFor(i => i.Quantity).NotEmpty()
                .GreaterThan(0)
                .WithMessage("Quantity's value must be greater than 0");
        }
    }
}
