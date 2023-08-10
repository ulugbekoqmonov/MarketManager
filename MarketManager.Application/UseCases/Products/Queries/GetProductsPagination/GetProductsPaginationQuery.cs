using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Products.Response;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Products.Queries.GetAllProductsWithPagination
{
    public record GetProductsPaginationQuery : IRequest<PaginatedList<ProductResponse>>
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public class GetProductsPaginationQueryHandler : IRequestHandler<GetProductsPaginationQuery,
        PaginatedList<ProductResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetProductsPaginationQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<ProductResponse>> Handle(
            GetProductsPaginationQuery request, CancellationToken cancellationToken)
        {
            var search = request.SearchTerm?.Trim();
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(s => s.Name.ToLower().Contains(search.ToLower())
                                            || s.Description.ToLower().Contains(search.ToLower()));
            }
            if (products is null || products.Count() <= 0)
            {
                throw new NotFoundException(nameof(Product), search);
            }

            var paginatedProducts = await PaginatedList<Product>.CreateAsync(
                products, request.PageNumber, request.PageSize);

            var response = _mapper.Map<List<ProductResponse>>(paginatedProducts.Items);

            var result = new PaginatedList<ProductResponse>
                (response, paginatedProducts.TotalCount, request.PageNumber, request.PageSize);

            return result;
        }
    }
}
