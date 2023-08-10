using FluentValidation;

namespace MarketManager.Application.UseCases.Suppliers.Commands.DeleteSupplier
{
    public class DeleteSupplierCommandValidator : AbstractValidator<DeleteSupplierCommand>
    {
        public DeleteSupplierCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
