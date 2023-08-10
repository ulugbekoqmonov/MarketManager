using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Orders.Response;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Orders.Queries.GetOrdersWithFilter
{
    public class FilterOrderQuery : IRequest<PaginatedList<OrderResponse>>
    {
        public double? ItemAmount { get; set; }
        public string? ProductName { get; set; }
        public double? MinTotalValue { get; set; }
        public double? MaxTotalValue { get; set; }
        public double? MinTotalPriceBeforeCashback { get; set; }
        public double? MaxTotalPriceBeforeCashback { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class FilterOrderQueryHandler : IRequestHandler<FilterOrderQuery, PaginatedList<OrderResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FilterOrderQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<OrderResponse>> Handle(FilterOrderQuery request, CancellationToken cancellationToken)
        {
            var pageSize = request.PageSize;
            var pageNumber = request.PageNumber;
            var orders = _context.Orders.AsQueryable();

            if(request.ItemAmount is not null)
            {
                var items = _context.Items.Where(i => i.Amount == request.ItemAmount);
                orders = orders.Where(o => items.Any(i=>o.Items.Contains(i)));
            }
            if(request.ProductName is not null)
            {
                var items = _context.Items.Where(i => i.Product.Name == request.ProductName);
                orders = orders.Where(o => items.Any(i => o.Items.Contains(i)));
            }
            if (request.EndDate is null)
                request.EndDate = DateOnly.FromDateTime(DateTime.Now);

            if (!request.StartDate.HasValue)
            {
                orders = orders.Where(date => DateOnly.FromDateTime(date.CreatedDate) <= request.EndDate);
            }
            else
            {
                orders = orders.Where(date => DateOnly.FromDateTime(date.CreatedDate) >= request.StartDate
                 && DateOnly.FromDateTime(date.CreatedDate) <= request.EndDate);
            }

            if (request.MinTotalValue is not null)
                orders = orders.Where(x => x.TotalPrice >= request.MinTotalValue);

            if (request.MaxTotalValue is not null)
                orders = orders.Where(x => x.TotalPrice <= request.MaxTotalValue);

            if (request.MinTotalPriceBeforeCashback is not null)
                orders = orders.Where(x => x.TotalPriceBeforeCashback >= request.MinTotalPriceBeforeCashback);

            if (request.MaxTotalPriceBeforeCashback is not null)
                orders = orders.Where(x => x.TotalPriceBeforeCashback <= request.MaxTotalPriceBeforeCashback);


            return await GetPaginated(request, pageSize, pageNumber, orders);
        }

        private async Task<PaginatedList<OrderResponse>> GetPaginated(FilterOrderQuery request, int pageSize, int pageNumber, IQueryable<Order> orders)
        {
            var paginatedOrder = await PaginatedList<Order>.CreateAsync(orders, pageNumber, pageSize);
            var responseOrder = _mapper.Map<List<OrderResponse>>(paginatedOrder.Items);
            var result = new PaginatedList<OrderResponse>
                (responseOrder, paginatedOrder.TotalCount, request.PageNumber, request.PageSize);
            return result;
        }

    }
}
