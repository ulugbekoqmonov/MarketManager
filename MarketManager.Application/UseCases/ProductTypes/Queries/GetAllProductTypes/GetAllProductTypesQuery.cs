using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.ProductTypes.Response;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.ProductTypes.Queries.GetAllProductTypes;

public record GetAllProductTypesQuery : IRequest<PaginatedList<ProductTypeResponce>>
{
    public string? SearchingText { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllProductTypesQueryHandler : IRequestHandler<GetAllProductTypesQuery, PaginatedList<ProductTypeResponce>>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetAllProductTypesQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<PaginatedList<ProductTypeResponce>> Handle(GetAllProductTypesQuery request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize;
        var pageNumber = request.PageNumber;
        var searchingText = request.SearchingText?.Trim();

        var productTypes = _context.ProductTypes.AsQueryable();

        if (!string.IsNullOrEmpty(searchingText))
        {
            productTypes = productTypes.Where(p => p.Name.ToLower().Contains(searchingText.ToLower()));
        }
        if (productTypes is null || productTypes.Count() <= 0)
        {
            throw new NotFoundException(nameof(Permission), searchingText);
        }

        var paginatedProductTypes = await PaginatedList<ProductType>.CreateAsync(productTypes, pageNumber, pageSize);

        var productTypeResponces = _mapper.Map<List<ProductTypeResponce>>(paginatedProductTypes.Items);

        var result = new PaginatedList<ProductTypeResponce>(productTypeResponces, 
            paginatedProductTypes.TotalCount, request.PageNumber, request.PageSize);

        return result;
    }

}



