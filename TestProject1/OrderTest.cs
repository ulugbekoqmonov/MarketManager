using MarketManager.API;
using MarketManager.Application.UseCases.Orders.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace TestProject1;

public class OrderTest:IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public OrderTest(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }
    [Theory]
    [InlineData("8da8ce69-fb58-440b-b65d-f074dd0c413e")]
    [InlineData("8da8ce69-fb58-440b-b65d-f074dd0c413f")]
    [InlineData("8da8ce69-fb58-440b-b65d-f074dd0c413fssd")]
    [InlineData("8da8ce69-fb58-440sdsdds")]
    [InlineData("")]
    public async Task GetByIdOrderFail(string id)
    {
        // Arrange: Set up the request
        var request = $"/api/Order/GetOrderById?Id={id}";

        // Act: Send the HTTP request
        var response = await _httpClient.GetAsync(request);                        
        var order = response.Content.ReadFromJsonAsync<OrderResponse>();

        //Assert
        Assert.Equal(id, order.Id.ToString());
        Assert.IsType<OrderResponse>(response);        
    }

    [Theory]
    [InlineData("")]
    public async Task GetByIdOrderSuccess(string id)
    {
        // Arrange: Set up the request
        var request = $"/api/Order/GetOrderById?Id={id}";

        // Act: Send the HTTP request
        var response = await _httpClient.GetAsync(request);
        var order = response.Content.ReadFromJsonAsync<OrderResponse>();

        //Assert
        
    }

    [Theory]
    [InlineData]
    public async Task CreateOrderFail()
    {

    }

    [Theory]
    [InlineData]
    public async Task CreateOrderSuccess()
    {

    }

}
