using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
namespace MarketManager.Application.UseCases.Clients.Commands.DeleteClient;

public record DeleteClientCommand(Guid Id) : IRequest;
public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteClientCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        Client? client = await _dbContext.Clients.FindAsync(request.Id);

        if (client is null)
        {
            throw new NotFoundException(nameof(client), request.Id);
        }

        _dbContext.Clients.Remove(client);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
