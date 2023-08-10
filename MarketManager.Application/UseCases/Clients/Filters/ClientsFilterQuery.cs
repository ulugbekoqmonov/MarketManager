using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Clients.Queries.GetAllClients;
using MarketManager.Domain.Entities;
using MediatR;
namespace MarketManager.Application.UseCases.Clients.Filters;

public class ClientsFilterQuery : IRequest<PaginatedList<GetAllClientsQueryResponse>>
{
    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool CashBackSum { get; set; } = false;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

}
public class ClientsFilterQueryHandler : IRequestHandler<ClientsFilterQuery, PaginatedList<GetAllClientsQueryResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ClientsFilterQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetAllClientsQueryResponse>> Handle(ClientsFilterQuery request, CancellationToken cancellationToken)
    {
        var allClients = _context.Clients.AsQueryable();
        if (request.StartDate is null)
            request.StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-30));


        if (request.EndDate is null)
            request.EndDate = DateOnly.FromDateTime(DateTime.Now);


        if (request.CashBackSum)
        {
            allClients = allClients
           .Where(date => DateOnly.FromDateTime(date.CreatedDate) >= request.StartDate && DateOnly.FromDateTime(date.CreatedDate) <= request.EndDate)
           .OrderByDescending(x => x.CashbackSum);
        }
        if (!request.CashBackSum)
        {
            allClients = allClients
             .Where(date => DateOnly.FromDateTime(date.CreatedDate) >= request.StartDate && DateOnly.FromDateTime(date.CreatedDate) <= request.EndDate)
            .OrderByDescending(x => x.CashbackSum);
        }
        var response = allClients.Select(p => _mapper.Map<Client, GetAllClientsQueryResponse>(p));
        return await PaginatedList<GetAllClientsQueryResponse>.CreateAsync(response, request.PageNumber, request.PageSize);

    }
}
