using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.UseCases.Feedbacks.Commands.UpdateFeedback;
public class UpdateFeedbackCommandValidator:AbstractValidator<UpdateFeedbackCommand>
{
    public UpdateFeedbackCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Invalid email address.");

        RuleFor(x => x.Phone)
               .NotEmpty().WithMessage("Phone is required.")
               .Matches(@"^\+998(33|9[0-9])\d{7}$")
                   .WithMessage("Phone must be in the format of '+998 90 123 45 67'.");

        RuleFor(x => x.Message)
            .NotEmpty().MinimumLength(10);
    }
}
