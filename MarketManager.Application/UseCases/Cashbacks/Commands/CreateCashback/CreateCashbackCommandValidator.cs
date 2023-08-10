using FluentValidation;
namespace MarketManager.Application.UseCases.Cashbacks.Commands.CreateCashback
{
    public class CreateCashbackCommandValidator: AbstractValidator<CreateCashbackCommand>
    {
        public CreateCashbackCommandValidator()
        {
            RuleFor(cashback => cashback.CashbackPercent).NotEmpty();
        }
    }
}
