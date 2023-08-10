using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MarketManager.Domain.States;
using MediatR;

namespace MarketManager.Application.UseCases.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public double Discount { get; set; }
        public double SalePrice { get; private set; }
        public MeasureTypes MeasureType { get; set; }
        public string Description { get; set; }
        public string? Picture { get; set; }
        public Guid ProductTypeId { get; set; }
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product? Product = await _context.Products.FindAsync(request.Id);
            _mapper.Map(Product, request);

            if (Product is null)
                throw new NotFoundException(nameof(Product), request.Id);

            var productType = await _context.ProductTypes.FindAsync(request.ProductTypeId);

            if (productType is null)
                throw new NotFoundException(nameof(ProductType), request.ProductTypeId);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
