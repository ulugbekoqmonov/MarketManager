using AutoMapper;
using MarketManager.Application.UseCases.Clients.Commands.CreateClient;
using MarketManager.Application.UseCases.Clients.Commands.DeleteClient;
using MarketManager.Application.UseCases.Clients.Commands.UpdateClient;
using MarketManager.Application.UseCases.Clients.Queries.GetAllClients;
using MarketManager.Application.UseCases.Clients.Queries.GetClientById;
using MarketManager.Application.UseCases.Clients.Reports;
using MarketManager.Domain.Entities;
namespace MarketManager.Application.Common.Mappings;

public class ClientMapping : Profile
{
    public ClientMapping()
    {
        ClientMap();
    }

    private void ClientMap()
    {
        CreateMap<CreateClientCommand, Client>();
        CreateMap<UpdateClientCommand, Client>();
        CreateMap<DeleteClientCommand, Client>();
        CreateMap<Client, GetAllClientsQueryResponse>().ReverseMap();
        CreateMap<Client, GetClientByIdQueryResponse>().ReverseMap();
        CreateMap<Client, ClientsResponseExcelReport>().ReverseMap();
    }
}
