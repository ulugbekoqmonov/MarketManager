using AutoMapper;
using MarketManager.Application.UseCases.Orders.Commands.CreateOrder;
using MarketManager.Application.UseCases.Orders.Commands.DeleteOrder;
using MarketManager.Application.UseCases.Orders.Commands.UpdateOrder;
using MarketManager.Application.UseCases.Orders.Response;
using MarketManager.Domain.Entities;

namespace MarketManager.Application.Common.Mappings;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        OrderMappings();
        OrderWithCart();
    }

    private void OrderWithCart()
    {
        CreateMap<CreateOrderCommand, Order>().ReverseMap();
        CreateMap<UpdateOrderCommand, Order>();
        CreateMap<DeleteOrderCommand, Order>();
    }

    private void OrderMappings()
    {
        CreateMap<Order, OrderResponse>();
    }
}
