using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
namespace MarketManager.Application.UseCases.Clients.Commands.CreateClient;

public class CreateClientCommand : IRequest<Guid>
{
    public string? PhoneNumber { get; set; }
    public double CashbackSum { get; set; }
    public string CardNumber { get; set; }

}
public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateClientCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        Client client = _mapper.Map<Client>(request);

        await _dbContext.Clients.AddAsync(client, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return client.Id;
    }
}
