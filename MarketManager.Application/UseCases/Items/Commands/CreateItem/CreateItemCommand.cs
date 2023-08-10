using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Items.Commands.CreateItem
{
    public class CreateItemCommand : IRequest<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public double Amount { get; set; }
        public double TotalPrice { get; set; }
    }
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateItemCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Guid> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            Item item = _mapper.Map<Item>(request);
            await _context.Items.AddAsync(item, cancellationToken);
            await _context.SaveChangesAsync();
            return item.Id;
        }
    }
}
