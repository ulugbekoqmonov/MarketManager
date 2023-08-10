using AutoMapper;
using MarketManager.Application.UseCases.Cashbacks.Commands.CreateCashback;
using MarketManager.Application.UseCases.Cashbacks.Commands.DeleteCashback;
using MarketManager.Application.UseCases.Cashbacks.Commands.UpdateCashback;
using MarketManager.Application.UseCases.Cashbacks.Queries.GetAllCashbacks;
using MarketManager.Application.UseCases.Cashbacks.Queries.GetCashbackById;
using MarketManager.Domain.Entities;
namespace MarketManager.Application.Common.Mappings;

public class CashbackMapping : Profile
{
    public CashbackMapping()
    {
        CashbackMap();
    }

    private void CashbackMap()
    {
        CreateMap<CreateCashbackCommand, Cashback>();
        CreateMap<UpdateCashbackCommand, Cashback>();
        CreateMap<DeleteCashbackCommand, Cashback>();
        CreateMap<Cashback, GetAllCashbacksQueryResponse>().ReverseMap();
        CreateMap<Cashback, GetCashbackByIdQueryResponse>().ReverseMap();
    }
}
