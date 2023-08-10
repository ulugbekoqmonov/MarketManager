using AutoMapper;
using MarketManager.Application.UseCases.Products.Commands.CreateProduct;
using MarketManager.Application.UseCases.Products.Commands.DeleteProduct;
using MarketManager.Application.UseCases.Products.Commands.UpdateProduct;
using MarketManager.Application.UseCases.Products.Response;
using MarketManager.Domain.Entities;

namespace MarketManager.Application.Common.Mappings;
public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<CreateProductCommand, Product>().ReverseMap();
        CreateMap<DeleteProductCommand, Product>().ReverseMap();
        CreateMap<UpdateProductCommand, Product>().ReverseMap();
        CreateMap<ProductResponse, Product>().ReverseMap();
    }
}
