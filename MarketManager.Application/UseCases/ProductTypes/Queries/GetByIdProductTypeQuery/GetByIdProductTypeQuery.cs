using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.ProductTypes.Response;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.ProductTypes.Queries.GetByIdProductTypeQuery;

public record GetByIdProductTypeQuery(Guid Id) : IRequest<ProductTypeResponce>;

public class GetByIdProductTypeQueryHandler : IRequestHandler<GetByIdProductTypeQuery, ProductTypeResponce>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetByIdProductTypeQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<ProductTypeResponce> Handle(GetByIdProductTypeQuery request, CancellationToken cancellationToken)
    {
        var productType = await _context.ProductTypes.FindAsync(request.Id);
        if (productType is null)
            throw new NotFoundException(nameof(ProductsType), request.Id);

        return _mapper.Map<ProductTypeResponce>(productType);
    }

}
