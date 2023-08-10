using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Products.Response;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Products.Queries.GetAllProductsWithPagination
{
    public record GetAllProductsFilterQuery : IRequest<PaginatedList<ProductResponse>>
    {
        public string Category { get; set; } = string.Empty;
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string SearchingText { get; } = string.Empty;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public class GetAllProductsFilterQueryHandler : IRequestHandler<GetAllProductsFilterQuery, PaginatedList<ProductResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetAllProductsFilterQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<ProductResponse>> Handle(GetAllProductsFilterQuery request, CancellationToken cancellationToken)
        {
            var allProducts = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(request.SearchingText))
            {
                allProducts = allProducts.Where(p => p.Name.Contains(request.SearchingText));
            }

            if (!string.IsNullOrEmpty(request.Category))
            {
                allProducts = allProducts.Where(p => p.ProductType.Equals(request.Category));
            }

            if (request.MinPrice.HasValue)
            {
                // Filter based on the minimum sale price in any package associated with the product
              //  allProducts = allProducts.Where(p => p.Packages.Any(package => package.SalePriceBeforeDiscount >= request.MinPrice.Value));
            }

            if (request.MaxPrice.HasValue)
            {
                // Filter based on the maximum sale price in any package associated with the product
              //  allProducts = allProducts.Where(p => p.Packages.Any(package => package.SalePriceBeforeDiscount <= request.MaxPrice.Value));
            }

            var paginatedProducts = await PaginatedList<Product>.CreateAsync(allProducts, request.PageNumber, request.PageSize);
            var response = _mapper.Map<PaginatedList<Product>, PaginatedList<ProductResponse>>(paginatedProducts);
            return response;
        }
    }
}
