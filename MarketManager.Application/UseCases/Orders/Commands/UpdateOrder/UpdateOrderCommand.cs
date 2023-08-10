using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketManager.Application.UseCases.Orders.Commands.UpdateOrder;

public class UpdateOrderCommand : IRequest
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }

    public Guid ClientId { get; set; }
    public double CashbackSum { get; set; }

    public double TotalPriceBeforeCashback { get; set; }

}
public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public UpdateOrderCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = await FilterIfOrderExists(request.Id);
        _mapper.Map(request, order);
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Order> FilterIfOrderExists(Guid id)
     => await _dbContext.Orders//.Include("Items")
                .FirstOrDefaultAsync(x => x.Id == id)
                 ?? throw new NotFoundException(
                          " there is no order with this id. ");
}
