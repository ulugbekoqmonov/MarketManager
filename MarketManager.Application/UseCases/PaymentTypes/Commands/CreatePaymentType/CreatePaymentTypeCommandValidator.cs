using FluentValidation;

namespace MarketManager.Application.UseCases.PaymentTypes.Commands.CreatePaymentType
{
    public class CreatePaymentTypeCommandValidator : AbstractValidator<CreatePaymentTypeCommand>
    {
        public CreatePaymentTypeCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
