using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
namespace MarketManager.Application.UseCases.Cashbacks.Queries.GetCashbackById;
public record GetCashbackByIdQuery(Guid Id) : IRequest<GetCashbackByIdQueryResponse>;
public class GetCashbackByIdQueryHandler : IRequestHandler<GetCashbackByIdQuery, GetCashbackByIdQueryResponse>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetCashbackByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<GetCashbackByIdQueryResponse> Handle(GetCashbackByIdQuery request, CancellationToken cancellationToken)
    {
        Cashback? Cashback = await _context.Cashbacks.FindAsync(request.Id);
        if (Cashback is null)
            throw new NotFoundException(nameof(Cashback), request.Id);
        return _mapper.Map<GetCashbackByIdQueryResponse>(Cashback);
    }
}
public class GetCashbackByIdQueryResponse
{
    public Guid Id { get; set; }
    public sbyte CashbackPercent { get; set; }

}
