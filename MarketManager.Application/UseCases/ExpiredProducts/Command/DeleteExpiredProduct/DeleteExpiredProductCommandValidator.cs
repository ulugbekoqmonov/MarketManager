using FluentValidation;

namespace MarketManager.Application.UseCases.ExpiredProducts.Command.DeleteExpiredProduct
{
    public class DeleteExpiredProductCommandValidator : AbstractValidator<DeleteExpiredProductCommand>
    {
        public DeleteExpiredProductCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
        }
    }
}
