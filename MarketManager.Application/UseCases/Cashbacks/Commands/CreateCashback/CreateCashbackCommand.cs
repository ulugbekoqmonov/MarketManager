using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
namespace MarketManager.Application.UseCases.Cashbacks.Commands.CreateCashback
{
    public class CreateCashbackCommand : IRequest<Guid>
    {
        public sbyte CashbackPercent { get; set; }
    }
    public class CreateCashbackCommandHandler : IRequestHandler<CreateCashbackCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateCashbackCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateCashbackCommand request, CancellationToken cancellationToken)
        {
            Cashback Cashback = _mapper.Map<Cashback>(request);

            await _dbContext.Cashbacks.AddAsync(Cashback, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Cashback.Id;
        }
    }

}
