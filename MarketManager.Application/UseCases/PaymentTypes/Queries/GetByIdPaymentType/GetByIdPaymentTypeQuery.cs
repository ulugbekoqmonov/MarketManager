using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.PaymentTypes.Responce;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.PaymentTypes.Queries.GetByIdPaymentType
{
    public class GetByIdPaymentTypeQuery : IRequest<GetPaymentTypeQueryResponse>
    {
        public Guid Id { get; set; }
    }
    public class GetByIdPaymentTypeQueryHandler : IRequestHandler<GetByIdPaymentTypeQuery, GetPaymentTypeQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetByIdPaymentTypeQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GetPaymentTypeQueryResponse> Handle(GetByIdPaymentTypeQuery request, CancellationToken cancellationToken)
        {
            PaymentType? paymentType = await _context.PaymentTypes.FindAsync(request.Id);
            if (paymentType == null) throw new NotFoundException(nameof(PaymentType), request.Id);

            GetPaymentTypeQueryResponse? mapped = _mapper.Map<GetPaymentTypeQueryResponse>(paymentType);
            return mapped;
        }
    }

    
}
