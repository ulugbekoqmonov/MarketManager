using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Items.Response;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Items.Queries.GetAllItems
{
    public record GetAllItemsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<IEnumerable<ItemResponse>>;

    public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, IEnumerable<ItemResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetAllItemsQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<ItemResponse>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Item> Items = _context.Items;

            return await Task.FromResult(_mapper.Map<IEnumerable<ItemResponse>>(Items));
        }
    }
}
