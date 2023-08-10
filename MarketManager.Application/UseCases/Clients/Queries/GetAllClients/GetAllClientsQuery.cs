using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Domain.Entities;
using MediatR;
namespace MarketManager.Application.UseCases.Clients.Queries.GetAllClients;

public record GetAllClientsQuery : IRequest<PaginatedList<GetAllClientsQueryResponse>>
{
    public string? SearchingText { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, PaginatedList<GetAllClientsQueryResponse>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetAllClientsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedList<GetAllClientsQueryResponse>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize;
        var pageNumber = request.PageNumber;
        var searchingText = request.SearchingText?.Trim();

        var clients =  _context.Clients.AsQueryable();
        if (!string.IsNullOrEmpty(searchingText))
        {
            clients = clients.Where(p=> p.CashbackSum.ToString().ToLower().Contains(searchingText.ToLower()));
        }
        if (clients is null || clients.Count() <= 0)
            throw new NotFoundException(nameof(Client), searchingText);
        var paginatedClients = await PaginatedList<Client>.CreateAsync(clients, pageNumber, pageSize);

        var clientResponses = _mapper.Map<List<GetAllClientsQueryResponse>>(paginatedClients.Items);

        var result = new PaginatedList<GetAllClientsQueryResponse>(clientResponses, paginatedClients.TotalCount, paginatedClients.PageNumber, paginatedClients.TotalPages);
        return result;

    }
}
public class GetAllClientsQueryResponse
{
    public Guid Id { get; set; }
    public string? PhoneNumber { get; set; }
    public double CashbackSum { get; set; }
    public string CardNumber { get; set; }
}
