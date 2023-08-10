using FluentValidation;
using MarketManager.Application.UseCases.CompanysData.Command.DeleteCompanyData;

namespace MarketManager.Application.UseCases.CompanyData.Command.DeleteCompanyData
{
    public class DeleteCompanyDataCommandValidator : AbstractValidator<DeleteCompanyDataCommand>
    {
        public DeleteCompanyDataCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
