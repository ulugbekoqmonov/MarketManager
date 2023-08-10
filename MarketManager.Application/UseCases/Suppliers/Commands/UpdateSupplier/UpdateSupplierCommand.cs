using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Suppliers.Commands.UpdateSupplier
{
    public record UpdateSupplierCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UpdateSupplierCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var foundSupplier = await _context.Suppliers.FindAsync(request.Id, cancellationToken);
            _mapper.Map(foundSupplier, request);

            if (foundSupplier is null)
                throw new NotFoundException(nameof(Supplier), request.Id);

            foundSupplier.Name = request.Name;
            foundSupplier.Phone = request.Phone;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
