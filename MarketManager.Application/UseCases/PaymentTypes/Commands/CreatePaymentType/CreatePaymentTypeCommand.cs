using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.PaymentTypes.Commands.CreatePaymentType
{
    public class CreatePaymentTypeCommand : IRequest<Guid>
    {
        public string Name { get; set; }
    }
    public class CreatePaymentTypeCommandHandler : IRequestHandler<CreatePaymentTypeCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreatePaymentTypeCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Guid> Handle(CreatePaymentTypeCommand request, CancellationToken cancellationToken)
        {
            PaymentType? pt = _mapper.Map<PaymentType>(request);
            await _context.PaymentTypes.AddAsync(pt);
            await _context.SaveChangesAsync();
            return pt.Id;
        }
    }
}
