using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.ExpiredProducts.Command.CreateExpiredProduct
{
    public class CreateExpiredProductCommand : IRequest<Guid>
    {
        public Guid PackageId { get; set; }
        public int Count { get; set; }
    }

    public class CreateExpiredProductCommandHandler : IRequestHandler<CreateExpiredProductCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateExpiredProductCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Guid> Handle(CreateExpiredProductCommand request, CancellationToken cancellationToken)
        {
            ExpiredProduct expiredProduct = _mapper.Map<ExpiredProduct>(request);
            await _context.ExpiredProducts.AddAsync(expiredProduct, cancellationToken);
            await _context.SaveChangesAsync();

            return expiredProduct.Id;

        }
    }

}
