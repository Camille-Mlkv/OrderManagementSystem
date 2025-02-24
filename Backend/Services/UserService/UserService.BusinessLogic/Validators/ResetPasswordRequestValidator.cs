using FluentValidation;
using UserService.BusinessLogic.DTOs.Requests;

namespace UserService.BusinessLogic.Validators
{
    public class ResetPasswordRequestValidator: AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email field is required.")
            .EmailAddress().WithMessage("Wrong email.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password field is required.")
                .MinimumLength(8).WithMessage("Password must contain at least 8 symbols.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one digit.")
                .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Password confirmation field is required.")
                .Equal(x => x.Password).WithMessage("Passwords don't match.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Confirmation code field is required.");
        }
    }
}
