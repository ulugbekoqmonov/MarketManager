using AutoMapper;
using MarketManager.Application.UseCases.Roles.Response;
using MarketManager.Domain.Entities.Identity;

namespace MarketManager.Application.Common.Mappings;
public class RoleMapping : Profile
{
    public RoleMapping()
    {
        CreateMap<RoleResponse, Role>().ReverseMap();
    }
}
