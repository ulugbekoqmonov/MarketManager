using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.PaymentTypes.Responce;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.PaymentTypes.Queries.GetAllPaymentType
{
    public class GetAllPaymentTypeQuery : IRequest<PaginatedList<GetPaymentTypeQueryResponse>>
    {
        public string? SearchingText { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllPaymentTypeQueryHandler : IRequestHandler<GetAllPaymentTypeQuery, PaginatedList<GetPaymentTypeQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetAllPaymentTypeQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<GetPaymentTypeQueryResponse>> Handle(GetAllPaymentTypeQuery request, CancellationToken cancellationToken)
        {

            var pageSize = request.PageSize;
            var pageNumber = request.PageNumber;
            var searchingText = request.SearchingText;

            var paymentTypes = _context.PaymentTypes.AsQueryable();

            if (!string.IsNullOrEmpty(searchingText))
            {
                paymentTypes = paymentTypes.Where(p=> p.Name.ToLower().Contains(searchingText.ToLower()));
            }
            if (paymentTypes == null || paymentTypes.Count() < 0)
            {
                throw new NotFoundException(nameof(PaymentType), searchingText);
            }


            PaginatedList<PaymentType> paginatedPaymentTypes = await PaginatedList<PaymentType>.CreateAsync(paymentTypes, pageNumber, pageSize);
            List<GetPaymentTypeQueryResponse> response = _mapper.Map<List<GetPaymentTypeQueryResponse>>(paginatedPaymentTypes.Items);

            var result = new PaginatedList<GetPaymentTypeQueryResponse>
                (response, paginatedPaymentTypes.TotalCount, paginatedPaymentTypes.PageNumber, paginatedPaymentTypes.TotalPages);
            return result;
        }
    }

    
}
