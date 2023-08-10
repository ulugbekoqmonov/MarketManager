using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.UseCases.CompanyData.Command.UpdateCompanyData
{
    public class UpdateCompanyDataCommandValidator : AbstractValidator<UpdateCompanyDataCommand>
    {
        public UpdateCompanyDataCommandValidator() 
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

            RuleFor(user => user.Location)
                .NotEmpty().WithMessage("Company Location is required.")
                .MinimumLength(3)
                .MaximumLength(100);
        }
    }
}
