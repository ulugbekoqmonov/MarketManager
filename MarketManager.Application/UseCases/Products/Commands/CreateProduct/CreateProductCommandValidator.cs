using FluentValidation;
using MarketManager.Domain.States;

namespace MarketManager.Application.UseCases.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(t => t.ProductTypeId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Product Type id is required.");

            RuleFor(d => d.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Name is required");

            RuleFor(d => d.Description)
                .NotEmpty()
                .MaximumLength(250)
                .WithMessage("Description is required");

            RuleFor(d => d.Barcode)
               .NotEmpty()
               .MaximumLength(250)
               .WithMessage("Barcode is required");

            RuleFor(d => d.MeasureType)
                .IsInEnum()
                .WithMessage("Invalid MeasureType");
        }
    }
}
