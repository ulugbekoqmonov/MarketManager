using FluentValidation;
namespace MarketManager.Application.UseCases.Clients.Commands.CreateClient;
public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        //RuleFor(client => client.PhoneNumber).Matches(@"^\+998(33|9[0-9])\d{7}$")
        //        .WithMessage("Phone must be in the format of '+998 90 123 45 67'.");
        RuleFor(client => client.CashbackSum).GreaterThanOrEqualTo(0);
        RuleFor(client=> client.CardNumber).NotEmpty();
    }
}
