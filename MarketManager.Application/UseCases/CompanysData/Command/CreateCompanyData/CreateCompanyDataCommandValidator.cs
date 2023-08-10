using FluentValidation;
using MarketManager.Application.UseCases.CompanysData.Command.CreateCompanyData;

namespace MarketManager.Application.UseCases.CompanyData.Command.CreateCompanyData
{
    public class CreateCompanyDataCommandValidator : AbstractValidator<CreateCompanyDataCommand>
    {
        public CreateCompanyDataCommandValidator()
        {

            RuleFor(user => user.CompanyName)
                 .NotEmpty().WithMessage("CompanyName is required.")
                 .MinimumLength(3)
                 .MaximumLength(100);

            RuleFor(user => user.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^\+998(33|9[0-9])\d{7}$")
                .WithMessage("Phone must be in the format of '+998 90 123 45 67'.");

            RuleFor(compData => compData.Data).NotEmpty()
                .WithMessage("Data is required");

            RuleFor(compData => compData.LogoImg).NotEmpty()
                .WithMessage("LogoImg is required");

            RuleFor(user => user.Location)
                .NotEmpty().WithMessage("Company Location is required.")
                .MinimumLength(3)
                .MaximumLength(100);
        }
    }
}
