using AutoMapper;
using MarketManager.Application.UseCases.Suppliers.Commands.CreateSupplier;
using MarketManager.Application.UseCases.Suppliers.Commands.UpdateSupplier;
using MarketManager.Application.UseCases.Suppliers.Queries.GetAllSuppliers;
using MarketManager.Application.UseCases.Suppliers.Queries.GetSupplierById;
using MarketManager.Application.UseCases.Suppliers.Report;
using MarketManager.Domain.Entities;

namespace MarketManager.Application.Common.Mappings
{
    public class SupplierMapping : Profile
    {
        public SupplierMapping()
        {
            CreateMap<CreateSupplierCommand, Supplier>();
            CreateMap<UpdateSupplierCommand, Supplier>();
            CreateMap<GetSupplierByIdQuery, Supplier>().ReverseMap();
            CreateMap<GetAllSuppliersQueryResponse, Supplier>().ReverseMap();
            CreateMap<Supplier, SuppliersResponseExcelReport>().ReverseMap();
        }
    }
}
