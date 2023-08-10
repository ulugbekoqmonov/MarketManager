using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MarketManager.Domain.States;
using MediatR;

namespace MarketManager.Application.UseCases.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public double Discount { get; set; }
        public MeasureTypes MeasureType { get; set; }
        public string Description { get; set; }
        public string? Picture { get; set; }
        public Guid ProductTypeId { get; set; }
    }
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;

            _context = context;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product Product = _mapper.Map<Product>(request);
            await _context.Products.AddAsync(Product, cancellationToken);
            await _context.SaveChangesAsync();

            return Product.Id;
        }
    }
}
