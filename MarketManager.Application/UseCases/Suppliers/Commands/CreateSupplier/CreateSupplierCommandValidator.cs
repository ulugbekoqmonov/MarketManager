using FluentValidation;

namespace MarketManager.Application.UseCases.Suppliers.Commands.CreateSupplier
{
    public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierCommandValidator()
        {
            RuleFor(supplier => supplier.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(supplier => supplier.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^\+998(33|9[0-9])\d{7}$")
                .WithMessage("Phone must be in the format of '+998 90 123 45 67'.");
        }
    }
}
