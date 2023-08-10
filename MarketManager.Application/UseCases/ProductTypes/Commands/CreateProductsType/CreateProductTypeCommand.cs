using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.ProductTypes.Commands.CreateProductsType;

public class CreateProductTypeCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public IFormFile? Picture { get; set; }
}
public class CreateProductTypeCommandHandler : IRequestHandler<CreateProductTypeCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    public CreateProductTypeCommandHandler(IMapper mapper, IApplicationDbContext context, IConfiguration configuration)
    {
        _mapper = mapper;
        _context = context;
        _configuration = configuration;
    }

    public async Task<Guid> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
    {
        var productType = _mapper.Map<ProductType>(request);

        if (request.Picture is not null)
        {
            productType.Id = Guid.NewGuid();
            var productTypeImage = _configuration["ProductTypePicturePath"];
            string filename = productType.Id + Path.GetExtension(request.Picture.FileName);
            string productTypeImagePath = Path.Combine(productTypeImage, filename);
            using (var fs = new FileStream(productTypeImagePath, FileMode.Create))
            {
                await request.Picture.CopyToAsync(fs);
                productType.Picture = productTypeImagePath;
            };
        }

        await _context.ProductTypes.AddAsync(productType, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return productType.Id;

    }
}