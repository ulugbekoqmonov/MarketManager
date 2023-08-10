using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Orders.Response;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Orders.Queries.GetOrder;

public record GetOrderQuery(Guid Id) : IRequest<OrderResponse>;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderResponse>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetOrderQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<OrderResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = FilterIfOrderExsists(request.Id);

        var result = _mapper.Map<OrderResponse>(order);
        return await Task.FromResult(result);
    }

    private Order FilterIfOrderExsists(Guid id)
        => _dbContext.Orders
            .Find(id)
                 ?? throw new NotFoundException(
                        " There is no order with this Id. ");


}

