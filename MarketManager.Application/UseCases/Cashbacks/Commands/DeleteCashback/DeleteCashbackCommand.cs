using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
namespace MarketManager.Application.UseCases.Cashbacks.Commands.DeleteCashback;

public class DeleteCashbackCommand : IRequest
{
    public Guid Id { get; set; }
}
public class DeleteCashbackCommandHandler : IRequestHandler<DeleteCashbackCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteCashbackCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Handle(DeleteCashbackCommand request, CancellationToken cancellationToken)
    {
        Cashback? Cashback = await _dbContext.Cashbacks.FindAsync(request.Id);

        if (Cashback is null)
        {
            throw new NotFoundException(nameof(Cashback), request.Id);
        }

        _dbContext.Cashbacks.Remove(Cashback);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
