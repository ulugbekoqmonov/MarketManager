using AutoMapper;
using MarketManager.Application.Common.Abstraction;
using MarketManager.Application.UseCases.Clients.Queries.GetAllClients;
using MarketManager.Application.UseCases.ExpiredProducts.Queries;
using MarketManager.Application.UseCases.Products.Commands.DeleteProduct;
using MarketManager.Application.UseCases.Products.Commands.UpdateProduct;
using MarketManager.Application.UseCases.ProductTypes.Commands.CreateProductsType;
using MarketManager.Application.UseCases.ProductTypes.Response;
using MarketManager.Domain.Entities;

namespace MarketManager.Application.Common.Mappings;

public class ProductTypesMapping : Profile
{
    public ProductTypesMapping()
    {
        CreateMap<CreateProductTypeCommand, ProductType>().ReverseMap();
        CreateMap<UpdateProductCommand, ProductType>().ReverseMap();
        CreateMap<DeleteProductCommand, ProductType>().ReverseMap();
        CreateMap<GetAllClientsQueryResponse, ProductType>().ReverseMap();
        CreateMap<ExpiredProductResponce, ProductType>().ReverseMap();
        CreateMap<ProductTypeResponce, ProductType>().ReverseMap();
    }
}
