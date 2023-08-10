using AutoMapper;
using MarketManager.Application.Common.Abstraction;
using MarketManager.Application.UseCases.ExpiredProducts.Command.CreateExpiredProduct;
using MarketManager.Application.UseCases.ExpiredProducts.Command.DeleteExpiredProduct;
using MarketManager.Application.UseCases.ExpiredProducts.Command.UpdateExpiredProduct;
using MarketManager.Domain.Entities;

namespace MarketManager.Application.Common.Mappings.ExpiredProductMappings;

public class ExpiredProductsMappingProfile : Profile
{
    public ExpiredProductsMappingProfile()
    {
        CreateMap<CreateExpiredProductCommand, ExpiredProduct>().ReverseMap();
        CreateMap<UpdateExpiredProductCommand, ExpiredProduct>().ReverseMap();
        CreateMap<DeleteExpiredProductCommand, ExpiredProduct>().ReverseMap();

        CreateMap<ExpiredProduct, ExpiredProductResponce>()
            .ForMember(dest => dest.Product, src => src.MapFrom(expiredProducts => expiredProducts.Package.Product));
           
    }
}
