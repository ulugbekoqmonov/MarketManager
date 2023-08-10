using FluentValidation;
namespace MarketManager.Application.UseCases.Clients.Commands.UpdateClient;
public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(client => client.Id).NotEmpty();
        RuleFor(client => client.PhoneNumber).Matches(@"^\+998(33|9[0-9])\d{7}$")
               .WithMessage("Phone must be in the format of '+998 90 123 45 67'.");
    }
}
