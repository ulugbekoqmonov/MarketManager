using FluentValidation;

namespace MarketManager.Application.UseCases.Users.Commands.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {

        RuleFor(user => user.FullName)
         .NotEmpty().WithMessage("Full Name is required.")
         .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("should be not empty value")
            .MinimumLength(3)
            .MaximumLength(20);

        RuleFor(user => user.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .Matches(@"^\+998(33|9[0-9])\d{7}$")
                .WithMessage("Phone must be in the format of '+998 90 123 45 67'.");

        RuleFor(p => p.Password).NotEmpty().WithMessage("Your password cannot be empty")
                   .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                   .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                   .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                   .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                   .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
    }
}
