using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.ProductsType.Commands.DeleteProductType;

public class DeleteProductTypeCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteProductTypeCommandHandler : IRequestHandler<DeleteProductTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProductTypeCommand request, CancellationToken cancellationToken)
    {
        ProductType productType = await _context.ProductTypes.FindAsync(request.Id);
        if (productType == null)
        {
            throw new NotFoundException(nameof(ProductType), request.Id);
        }
        _context.ProductTypes.Remove(productType);
        await _context.SaveChangesAsync();
    }

}

