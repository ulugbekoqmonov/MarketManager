using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
namespace MarketManager.Application.UseCases.Cashbacks.Commands.UpdateCashback;

public class UpdateCashbackCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public sbyte CashbackPercent { get; set; }
}
public class UpdateCashbackCommandHandler : IRequestHandler<UpdateCashbackCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateCashbackCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateCashbackCommand request, CancellationToken cancellationToken)
    {
        var CashbackToUpdate = await _dbContext.Cashbacks.FindAsync(request.Id, cancellationToken);
        // _mapper.Map(CashbackToUpdate, request);
        if (CashbackToUpdate is null)
        {
            throw new NotFoundException(nameof(Cashback), request.Id);
        }

        //  CashbackToUpdate.TotalPrice = request.TotalPrice;
        CashbackToUpdate.CashbackPercent = request.CashbackPercent;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
