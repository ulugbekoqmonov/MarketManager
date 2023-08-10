using FluentValidation;

namespace MarketManager.Application.UseCases.ExpiredProducts.Command.UpdateExpiredProduct
{
    public class UpdateExpiredProductCommandValidator : AbstractValidator<UpdateExpiredProductCommand>
    {
        public UpdateExpiredProductCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.PackageId).NotEmpty();
            RuleFor(c => c.Count).NotEmpty();
        }
    }
}
