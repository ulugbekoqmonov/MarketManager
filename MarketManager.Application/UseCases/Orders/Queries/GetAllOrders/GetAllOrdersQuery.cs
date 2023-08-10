using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Orders.Response;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketManager.Application.UseCases.Orders.Queries.GetAllOrders;

public record GetAllOrderQuery(int PageNumber = 1, int PageSize = 10) : IRequest<List<OrderResponse>>;

public class GetallOrderCommmandHandler : IRequestHandler<GetAllOrderQuery, List<OrderResponse>>
{

    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetallOrderCommmandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<OrderResponse>> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
    {
        Order[] candidates = await _dbContext.Orders.ToArrayAsync();

        List<OrderResponse> dtos = _mapper.Map<OrderResponse[]>(candidates).ToList();

        return dtos;
    }
    
}
