using AutoMapper;
using MarketManager.Application.UseCases.Packages.Commands.CreatePackage;
using MarketManager.Application.UseCases.Packages.Commands.DeletePackage;
using MarketManager.Application.UseCases.Packages.Commands.UpdatePackage;
using MarketManager.Application.UseCases.Packages.Response;
using MarketManager.Domain.Entities;

namespace MarketManager.Application.Common.Mappings
{
    public class PackageMapping : Profile
    {
        public PackageMapping()
        {
            CreateMap<CreatePackageCommand, Package>().ReverseMap();
            CreateMap<DeletePackageCommand, Package>().ReverseMap();
            CreateMap<UpdatePackageCommand, Package>().ReverseMap();
            CreateMap<PackageResponse, Package>().ReverseMap();
        }
    }
}
