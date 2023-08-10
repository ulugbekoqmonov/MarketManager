using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Items.Response;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Items.Queries.GetItemById
{
    public record GetItemByIdQuery(Guid Id) : IRequest<ItemResponse>;

    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetItemByIdQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ItemResponse> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            Item? item = await _context.Items.FindAsync(request.Id);

            if (item is null)
                throw new NotFoundException(nameof(Item), request.Id);

            return _mapper.Map<ItemResponse>(item);
        }
    }
}
