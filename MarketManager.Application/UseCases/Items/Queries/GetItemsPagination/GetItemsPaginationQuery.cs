using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Items.Response;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Items.Queries.GetItemsPagination
{
    public class GetItemsPaginationQuery:IRequest<PaginatedList<ItemResponse>>
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public class GetItemsPaginationQueryHandler : IRequestHandler<GetItemsPaginationQuery, PaginatedList<ItemResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetItemsPaginationQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<ItemResponse>> Handle(
            GetItemsPaginationQuery request, CancellationToken cancellationToken)
        {
            var search = request.SearchTerm?.Trim();
            var items = _context.Items.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(i => i.Amount.ToString().Contains(search)
                                            || i.TotalPrice.ToString().Contains(search));
            }

            if (items is null || items.Count() <= 0)
            {
                throw new NotFoundException(nameof(Item), search);
            }

            var paginatedItems = await PaginatedList<Item>.CreateAsync(
                items, request.PageNumber, request.PageSize);

            var response = _mapper.Map<List<ItemResponse>>(paginatedItems);

            var result = new PaginatedList<ItemResponse>
               (response, paginatedItems.TotalCount, request.PageNumber, request.PageSize);

            return result;
        }
    }
}
