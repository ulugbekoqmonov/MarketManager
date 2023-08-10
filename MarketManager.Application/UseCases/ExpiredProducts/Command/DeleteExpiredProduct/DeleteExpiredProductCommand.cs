using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.ExpiredProducts.Command.DeleteExpiredProduct
{
    public class DeleteExpiredProductCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteExpiredProductHandler : IRequestHandler<DeleteExpiredProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteExpiredProductHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteExpiredProductCommand request, CancellationToken cancellationToken)
        {
            ExpiredProduct? expiredProduct = await _context.ExpiredProducts.FindAsync(request.Id);
            if (expiredProduct == null)
                throw new NotFoundException(nameof(ExpiredProduct), request.Id);

            _context.ExpiredProducts.Remove(expiredProduct);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }



}
