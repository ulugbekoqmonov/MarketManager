using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Orders.Response;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Orders.Queries.GetOrdersPagination;

public class GetOrdersPaginationQuery:IRequest<PaginatedList<OrderResponse>>
{
    public string? SearchTerm { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
public class GetOrdersPaginationQueryHandler : IRequestHandler<GetOrdersPaginationQuery, PaginatedList<OrderResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrdersPaginationQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<PaginatedList<OrderResponse>> Handle(GetOrdersPaginationQuery request, CancellationToken cancellationToken)
    {
        var search = request.SearchTerm?.Trim();
        var orders = _context.Orders.AsQueryable();        

        if (orders is null || orders.Count() <= 0)
        {
            throw new NotFoundException(nameof(Order), search);
        }

        var paginatedOrders = await PaginatedList<Order>.CreateAsync(
            orders, request.PageNumber, request.PageSize);

        var response = _mapper.Map<List<OrderResponse>>(paginatedOrders);

        var result = new PaginatedList<OrderResponse>
           (response, paginatedOrders.TotalCount, paginatedOrders.PageNumber, paginatedOrders.TotalPages);

        return result;
    }
}
