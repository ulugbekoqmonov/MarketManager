using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.ExpiredProducts.Command.UpdateExpiredProduct
{
    public class UpdateExpiredProductCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public int Count { get; set; }
    }

    public class UpdateExpiredProductCommandHandler : IRequestHandler<UpdateExpiredProductCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateExpiredProductCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateExpiredProductCommand request, CancellationToken cancellationToken)
        {
            ExpiredProduct? expiredProduct = await _context.ExpiredProducts.FindAsync(request.Id);
            _mapper.Map(expiredProduct, request);

            if (expiredProduct == null)
                throw new NotFoundException(nameof(ExpiredProduct), request.Id);

            var package = await _context.Packages.FindAsync(request.PackageId);
            if (package == null)
                throw new NotFoundException(nameof(Package), request.PackageId);

            _context.ExpiredProducts.Update(expiredProduct);
            await _context.SaveChangesAsync();
        }
    }
}
