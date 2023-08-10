using FluentValidation;

namespace MarketManager.Application.UseCases.PaymentTypes.Commands.DeletePaymentType
{
    public class DeletePaymentTypeCommandValidator : AbstractValidator<DeletePaymentTypeCommand>
    {
        public DeletePaymentTypeCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
