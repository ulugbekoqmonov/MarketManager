using FluentValidation;

namespace MarketManager.Application.UseCases.PaymentTypes.Commands.UpdatePaymentType
{
    public class UpdatePaymentTypeCommandValidator : AbstractValidator<UpdatePaymentTypeCommand>
    {
        public UpdatePaymentTypeCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
