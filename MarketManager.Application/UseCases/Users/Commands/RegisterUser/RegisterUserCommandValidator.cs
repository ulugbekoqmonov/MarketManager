using FluentValidation;

namespace MarketManager.Application.UseCases.Users.Commands.RegisterUser;
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(user => user.FullName).NotEmpty().WithMessage("Full name is required.");
        RuleFor(user => user.Username).NotEmpty().WithMessage("Username is required.");
        RuleFor(user => user.Phone).NotEmpty().WithMessage("Phone number is required.");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Your password cannot be empty")
                   .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                   .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                   .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                   .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                   .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
        RuleFor(user => user.ConfirmPassword)
            .Equal(user => user.Password).WithMessage("Passwords do not match.");
    }
}
