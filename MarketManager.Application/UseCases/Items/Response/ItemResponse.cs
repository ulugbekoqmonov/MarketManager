namespace MarketManager.Application.UseCases.Items.Response;

public class ItemResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid OrderId { get; set; }
    public double Amount { get; set; }
    public double TotalPrice { get; set; }
}
