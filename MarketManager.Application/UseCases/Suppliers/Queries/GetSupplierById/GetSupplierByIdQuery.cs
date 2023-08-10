using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Suppliers.Queries.GetSupplierById
{
    public record GetSupplierByIdQuery(Guid Id) : IRequest<GetSupplierByIdQueryRespоnse>;

    public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, GetSupplierByIdQueryRespоnse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetSupplierByIdQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GetSupplierByIdQueryRespоnse> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            Supplier? supplier = await _context.Suppliers.FindAsync(request.Id);

            if (supplier is null)
                throw new NotFoundException(nameof(Supplier), request.Id);

            return _mapper.Map<GetSupplierByIdQueryRespоnse>(supplier);
        }
    }

    public class GetSupplierByIdQueryRespоnse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
