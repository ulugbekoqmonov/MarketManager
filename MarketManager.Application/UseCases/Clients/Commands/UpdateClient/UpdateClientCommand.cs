using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
namespace MarketManager.Application.UseCases.Clients.Commands.UpdateClient;

public class UpdateClientCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string? PhoneNumber { get; set; }
    public double CashbackSum { get; set; }
    public string CardNumber { get; set; }
}
public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateClientCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var clientToUpdate = await _dbContext.Clients.FindAsync(request.Id, cancellationToken);
        // _mapper.Map(clientToUpdate, request);
        if (clientToUpdate is null)
        {
            throw new NotFoundException(nameof(Client), request.Id);
        }

      //  clientToUpdate.TotalPrice = request.TotalPrice;
        clientToUpdate.PhoneNumber = request.PhoneNumber;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
