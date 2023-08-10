using FluentValidation;
namespace MarketManager.Application.UseCases.Clients.Commands.DeleteClient;
public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
