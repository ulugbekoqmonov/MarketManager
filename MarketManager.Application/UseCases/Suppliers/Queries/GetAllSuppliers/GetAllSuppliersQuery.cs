using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Suppliers.Queries.GetAllSuppliers;

public record GetAllSuppliersQuery : IRequest<PaginatedList<GetAllSuppliersQueryResponse>>
{
    public string? SearchTerm { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
public class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, PaginatedList<GetAllSuppliersQueryResponse>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetAllSuppliersQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<PaginatedList<GetAllSuppliersQueryResponse>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
    {

        var search = request.SearchTerm?.Trim();
        var suppliers = _context.Suppliers.AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            suppliers = suppliers.Where(s => s.Name.ToLower().Contains(search.ToLower()) || s.Phone.Contains(search));
        }
        if (suppliers is null || suppliers.Count() <= 0)
        {
            throw new NotFoundException(nameof(Supplier), search);
        }


        var paginatedSuppliers = await PaginatedList<Supplier>.CreateAsync(suppliers, request.PageNumber, request.PageSize);

        var response = _mapper.Map<List<GetAllSuppliersQueryResponse>>(paginatedSuppliers.Items);
        var res = new PaginatedList<GetAllSuppliersQueryResponse>
            (response, paginatedSuppliers.TotalCount, paginatedSuppliers.PageNumber, paginatedSuppliers.TotalPages);
        return res;

    }
}
public class GetAllSuppliersQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
}
