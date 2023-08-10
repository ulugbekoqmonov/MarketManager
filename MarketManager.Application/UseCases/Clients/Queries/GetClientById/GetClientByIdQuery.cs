using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
namespace MarketManager.Application.UseCases.Clients.Queries.GetClientById;
public record GetClientByIdQuery(Guid Id) : IRequest<GetClientByIdQueryResponse>;
public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, GetClientByIdQueryResponse>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetClientByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<GetClientByIdQueryResponse> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        Client? client = await _context.Clients.FindAsync(request.Id);
        if (client is null)
            throw new NotFoundException(nameof(Client), request.Id);
        return _mapper.Map<GetClientByIdQueryResponse>(client);
    }
}
public class GetClientByIdQueryResponse
{
    public Guid Id { get; set; }
    public string? PhoneNumber { get; set; }
    public double CashbackSum { get; set; }
    public string CardNumber { get; set; }
}
