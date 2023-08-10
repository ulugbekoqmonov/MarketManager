using AutoMapper;
using MarketManager.Application.Common.Abstraction;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.ExpiredProducts.Queries.GetAllExpiredProducts
{
    public class GetAllExpiredProductsQuery : IRequest<PaginatedList<ExpiredProductResponce>>
    {
        public string? SearchingText { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

  
    public class GetAllExpiredProductsQueryHandler
                        : IRequestHandler<GetAllExpiredProductsQuery, PaginatedList<ExpiredProductResponce>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllExpiredProductsQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<ExpiredProductResponce>> Handle
                            (GetAllExpiredProductsQuery request, CancellationToken cancellationToken)
        {
            var pageSize = request.PageSize;
            var pageNumber = request.PageNumber;
            var searchingText = request.SearchingText;




            var expiredProducts = _context.ExpiredProducts.AsQueryable();
            if (!string.IsNullOrEmpty(searchingText))
            {
                expiredProducts = expiredProducts.Where(p => p.Package.Product.Name.ToLower().Contains(searchingText.ToLower()));

            }
            if (expiredProducts == null || expiredProducts.Count() < 0)
            {
                throw new NotFoundException(nameof(ExpiredProduct), searchingText);
            }
            PaginatedList<ExpiredProduct> paginatedExpiredProduct = await PaginatedList<ExpiredProduct>.CreateAsync(expiredProducts, pageNumber, pageSize);

            List<ExpiredProductResponce> expiredProductResponce = _mapper.Map<List<ExpiredProductResponce>>(paginatedExpiredProduct.Items);

            var result = new PaginatedList<ExpiredProductResponce>
                    (expiredProductResponce, paginatedExpiredProduct.TotalCount,
                    request.PageNumber, request.PageSize);
            return result;
        }
    }
}
