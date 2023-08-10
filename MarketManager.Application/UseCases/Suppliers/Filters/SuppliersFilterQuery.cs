using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Suppliers.Queries.GetAllSuppliers;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketManager.Application.UseCases.Suppliers.Filters;

public class SuppliersFilterQuery : IRequest<PaginatedList<GetAllSuppliersQueryResponse>>
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool OrderByName { get; set; } = false;
    public bool OrderByPhone { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
public class SuppliersFilterQueryHandler : IRequestHandler<SuppliersFilterQuery, PaginatedList<GetAllSuppliersQueryResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public SuppliersFilterQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetAllSuppliersQueryResponse>> Handle(SuppliersFilterQuery request, CancellationToken cancellationToken)
    {
        var allSuppliers = _context.Suppliers.AsQueryable();
        if (request.StartDate is null)
            request.StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-30));

        if (request.EndDate is null)
            request.EndDate = DateOnly.FromDateTime(DateTime.Now);

        if (request.OrderByName && !request.OrderByPhone)
        {
            allSuppliers = allSuppliers
                .Where(date => DateOnly.FromDateTime(date.CreatedDate) >= request.StartDate && DateOnly.FromDateTime(date.CreatedDate) <= request.EndDate)
                .OrderBy(x => x.Name);
        }
        if (request.OrderByPhone && !request.OrderByName)
        {
            allSuppliers = allSuppliers
                .Where(date => DateOnly.FromDateTime(date.CreatedDate) >= request.StartDate && DateOnly.FromDateTime(date.CreatedDate) <= request.EndDate)
                .OrderBy(x => x.Phone);
        }

        var response = allSuppliers.Select(p => _mapper.Map<Supplier, GetAllSuppliersQueryResponse>(p));
        return await PaginatedList<GetAllSuppliersQueryResponse>.CreateAsync(response, request.PageNumber, request.PageSize);
    }
}