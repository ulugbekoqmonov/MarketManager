using AutoMapper;
using MarketManager.Application.Common.Abstraction;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.ExpiredProducts.Filters
{

    public record GetExpiredProductFilterQuery : IRequest<PaginatedList<ExpiredProductResponce>>
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Name { get; set; }
        public string? ProductType { get; set; }
        public double? SalePrice { get; private set; }
        public int MeasureType { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
    public class GetExpiredProductFilterHandler : IRequestHandler<GetExpiredProductFilterQuery, PaginatedList<ExpiredProductResponce>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetExpiredProductFilterHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ExpiredProductResponce>> Handle(GetExpiredProductFilterQuery request, CancellationToken cancellationToken)
        {
            var pageSize = request.PageSize;
            var pageNumber = request.PageNumber;

            if (request.StartDate is null)
                request.StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-30));

            if (request.EndDate is null)
                request.EndDate = DateOnly.FromDateTime(DateTime.Now);

            var expirepProducts = _context.ExpiredProducts
                .Where(date => DateOnly.FromDateTime(date.CreatedDate) >= request.StartDate
                    && DateOnly.FromDateTime(date.CreatedDate) <= request.EndDate
                    || date.Package.Product.Name.ToUpper().Contains(request.Name.ToUpper())
                    || date.Package.Product.ProductType.Name.ToUpper().Contains(request.ProductType.ToUpper())
                    || date.Package.Product.SalePrice.Equals(request.SalePrice));
           
            return await GetPaginated(request, pageSize, pageNumber, expirepProducts);
        }

        private async Task<PaginatedList<ExpiredProductResponce>> GetPaginated
            (GetExpiredProductFilterQuery request, int pageSize, int pageNumber, IQueryable<ExpiredProduct> expirepProducts)
        {
            var paginatedExpirepProducts = await PaginatedList<ExpiredProduct>.CreateAsync(expirepProducts, pageNumber, pageSize);
            var responseExpirepProducts = _mapper.Map<List<ExpiredProductResponce>>(paginatedExpirepProducts.Items);
            var result = new PaginatedList<ExpiredProductResponce>
                (responseExpirepProducts, paginatedExpirepProducts.TotalCount, request.PageNumber, request.PageSize);
            return result;
        }
    }

}
