using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.PaymentTypes.Commands.UpdatePaymentType
{
    public class UpdatePaymentTypeCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdatePaymentTypeCommandHandler : IRequestHandler<UpdatePaymentTypeCommand>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdatePaymentTypeCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task Handle(UpdatePaymentTypeCommand request, CancellationToken cancellationToken)
        {
            PaymentType? paymentType = await _context.PaymentTypes.FindAsync(request.Id);
            if (paymentType == null) throw new NotFoundException(nameof(PaymentType), request.Id);

            PaymentType? mapped = _mapper.Map<PaymentType>(request);
            _context.PaymentTypes.Update(mapped);
            await _context.SaveChangesAsync();
        }
    }
}
