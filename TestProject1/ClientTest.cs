using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Clients.Commands.CreateClient;
using MarketManager.Domain.Entities;
using Moq;
namespace TestProject1;
public class ClientTest
{
    [Fact]
    public async Task CreateClientCommandHandler_ShouldCreateClient()
    {
        // Arrange
        var dbContextMock = new Mock<IApplicationDbContext>();
        var mapperMock = new Mock<IMapper>();

        var clientId = Guid.NewGuid();
        var cardNumber = "1234567898765432";
        var phoneNumber = "+998976543212";
        var cashbacksum = 1000;
        var client = new Client { Id = clientId, CardNumber = cardNumber, CashbackSum = cashbacksum, PhoneNumber = phoneNumber };

        var request = new CreateClientCommand();
        var cancellationToken = CancellationToken.None;

        mapperMock.Setup(m => m.Map<Client>(request)).Returns(client);

        var handler = new CreateClientCommandHandler(dbContextMock.Object, mapperMock.Object);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        Assert.Equal(clientId, result);
        dbContextMock.Verify(db => db.Clients.AddAsync(client, cancellationToken), Times.Once);
        dbContextMock.Verify(db => db.SaveChangesAsync(cancellationToken), Times.Once);
    }

}
