using AutoMapper;
using MarketManager.Application.Common.Abstraction;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.ExpiredProducts.Queries
{
    public record GetByIdExpiredProductsQuery(Guid Id) : IRequest<ExpiredProductResponce>;



    public class GetByIdExpiredProductsQueryHandler : IRequestHandler<GetByIdExpiredProductsQuery, ExpiredProductResponce>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetByIdExpiredProductsQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ExpiredProductResponce> Handle(GetByIdExpiredProductsQuery request, CancellationToken cancellationToken)
        {
            ExpiredProduct? getByIdExpiredProducts = await _context.ExpiredProducts.FindAsync(request.Id);
            if (getByIdExpiredProducts == null)
                throw new NotFoundException(nameof(ExpiredProduct), request.Id);

            return _mapper.Map<ExpiredProductResponce>(getByIdExpiredProducts);
        }
    }
}

