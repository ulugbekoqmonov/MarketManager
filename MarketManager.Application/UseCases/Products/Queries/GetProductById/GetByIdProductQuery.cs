using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Products.Response;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Products.Queries.GetByIdProduct
{
    public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse>;

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        IApplicationDbContext _dbContext;
        IMapper _mapper;

        public GetProductByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var Product = FilterIfProductExsists(request.Id);

            var result = _mapper.Map<ProductResponse>(Product);
            return await Task.FromResult(result);
        }

        private Product FilterIfProductExsists(Guid id)
            => _dbContext.Products
                .Find(id)
                     ?? throw new NotFoundException(
                            " There is no Product with this Id. ");
    }
}
