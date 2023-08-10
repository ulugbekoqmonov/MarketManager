using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Items.Commands.DeleteItem
{
    public class DeleteItemCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            Item? item = await _context.Items.FindAsync(request.Id);
            if (item is null)
                throw new NotFoundException(nameof(Item), request.Id);
        }
    }
}
