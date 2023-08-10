using AutoMapper;
using MarketManager.Application.UseCases.PaymentTypes.Commands.CreatePaymentType;
using MarketManager.Application.UseCases.PaymentTypes.Commands.DeletePaymentType;
using MarketManager.Application.UseCases.PaymentTypes.Commands.UpdatePaymentType;
using MarketManager.Application.UseCases.PaymentTypes.Queries.GetAllPaymentType;
using MarketManager.Application.UseCases.PaymentTypes.Queries.GetByIdPaymentType;
using MarketManager.Application.UseCases.PaymentTypes.Responce;
using MarketManager.Domain.Entities;

namespace MarketManager.Application.Common.Mappings
{
    public class PaymentTypeMapping : Profile
    {
        public PaymentTypeMapping()
        {
            CreateMap<CreatePaymentTypeCommand, PaymentType>().ReverseMap();
            CreateMap<DeletePaymentTypeCommand, PaymentType>().ReverseMap();
            CreateMap<UpdatePaymentTypeCommand, PaymentType>().ReverseMap();

            CreateMap<PaymentType, GetPaymentTypeQueryResponse>().ReverseMap();
            
        }
    }
}
