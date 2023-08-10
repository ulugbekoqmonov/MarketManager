using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Domain.Entities;
using MediatR;
namespace MarketManager.Application.UseCases.Cashbacks.Queries.GetAllCashbacks
{
    public record GetAllCashbacksQuery : IRequest<PaginatedList<GetAllCashbacksQueryResponse>>
    {
        public string? SearchingText { get; set; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public class GetAllCashbacksQueryHandler : IRequestHandler<GetAllCashbacksQuery, PaginatedList<GetAllCashbacksQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetAllCashbacksQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<GetAllCashbacksQueryResponse>> Handle(GetAllCashbacksQuery request, CancellationToken cancellationToken)
        {
            var pageSize = request.PageSize;
            var pageNumber = request.PageNumber;
            var searchingText = request.SearchingText?.Trim();

            var Cashbacks = _context.Cashbacks.AsQueryable();
            if (!string.IsNullOrEmpty(searchingText))
            {
                Cashbacks = Cashbacks.Where(p => p.CashbackPercent.ToString().ToLower().Contains(searchingText.ToLower()));
            }
            if (Cashbacks is null || Cashbacks.Count() <= 0)
                throw new NotFoundException(nameof(Cashback), searchingText);
            var paginatedCashbacks = await PaginatedList<Cashback>.CreateAsync(Cashbacks, pageNumber, pageSize);

            var CashbackResponses = _mapper.Map<List<GetAllCashbacksQueryResponse>>(paginatedCashbacks.Items);

            var result = new PaginatedList<GetAllCashbacksQueryResponse>(CashbackResponses, paginatedCashbacks.TotalCount, paginatedCashbacks.PageNumber, paginatedCashbacks.TotalPages);
            return result;

        }
    }
    public class GetAllCashbacksQueryResponse
    {
        public Guid Id { get; set; }
        public sbyte CashbackPercent { get; set; }
    }

}
