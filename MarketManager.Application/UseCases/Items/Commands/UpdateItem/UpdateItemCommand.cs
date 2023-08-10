using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Items.Commands.UpdateItem
{
    public class UpdateItemCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public double Amount { get; set; }
        public double TotalPrice { get; set; }
    }
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateItemCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            Item? item = await _context.Items.FindAsync(request.Id);
            _mapper.Map(request, item);
            if (item is null)
                throw new NotFoundException(nameof(Item), request.Id);
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product is null)
                throw new NotFoundException(nameof(Product), request.ProductId);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
