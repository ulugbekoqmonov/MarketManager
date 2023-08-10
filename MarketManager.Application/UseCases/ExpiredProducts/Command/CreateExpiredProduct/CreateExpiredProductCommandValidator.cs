using FluentValidation;

namespace MarketManager.Application.UseCases.ExpiredProducts.Command.CreateExpiredProduct
{
    public class CreateExpiredProductCommandValidator : AbstractValidator<CreateExpiredProductCommand>
    {
        public CreateExpiredProductCommandValidator()
        {
            RuleFor(exPro => exPro.PackageId)
                .NotEmpty()
                .WithMessage("PackageId is required");

            RuleFor(exPro => exPro.Count)
                .NotEmpty()
                .WithMessage("Count is required");

        }
    }
}
