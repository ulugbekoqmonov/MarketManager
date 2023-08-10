using AutoMapper;
using MarketManager.Application.UseCases.CompanysData.Command.CreateCompanyData;
using MarketManager.Application.UseCases.CompanysData.Responce;
using MarketManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.Common.Mappings
{
    public class CompanyDataMappingProfile : Profile
    {
        public CompanyDataMappingProfile()
        {
            CreateMap<CompanyDataResponce, CompanyData>().ReverseMap();
        }
    }
}
