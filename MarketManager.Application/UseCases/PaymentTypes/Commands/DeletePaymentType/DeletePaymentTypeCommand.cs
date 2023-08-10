using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.PaymentTypes.Commands.DeletePaymentType
{
    public class DeletePaymentTypeCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeletePaymentTypeCommandHandler : IRequestHandler<DeletePaymentTypeCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeletePaymentTypeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeletePaymentTypeCommand request, CancellationToken cancellationToken)
        {
            PaymentType? paymentType = await _context.PaymentTypes.FindAsync(request.Id);
            if (paymentType == null) throw new NotFoundException(nameof(PaymentType), request.Id);

            _context.PaymentTypes.Remove(paymentType);
            await _context.SaveChangesAsync();
        }
    }
}
