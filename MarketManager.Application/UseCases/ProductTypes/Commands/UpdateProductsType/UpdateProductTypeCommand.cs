using MarketManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.ProductsType.Commands.UpdateProductType;

public class UpdateProductTypeCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IFormFile? Picture { get; set; }
}
public class UpdateProductTypeCommandHandler : IRequestHandler<UpdateProductTypeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    public UpdateProductTypeCommandHandler(IApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task Handle(UpdateProductTypeCommand request, CancellationToken cancellationToken)
    {
        var productType = await _context.ProductTypes.FindAsync(request.Id);

        if (productType == null)
        {
            throw new NotFoundException(nameof(productType), request.Id);
        }
        productType.Name = request.Name;
        if (request.Picture is not null)
        {
            var productTypeImage = _configuration["ProductTypePicturePath"];
            string filename = productType.Id + Path.GetExtension(request.Picture.FileName);
            string productTypeImagePath = Path.Combine(productTypeImage, filename);
            using (var fs = new FileStream(productTypeImagePath, FileMode.Create))
            {
                await request.Picture.CopyToAsync(fs);
                productType.Picture = productTypeImagePath;
            };
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
