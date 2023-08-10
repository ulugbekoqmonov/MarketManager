using MarketManager.Application.UseCases.Items.Queries.GetItemById;
using MarketManager.Application.UseCases.Items.Response;

namespace MarketManager.Application.UseCases.Orders.Response;

public class OrderResponse
{
    public Guid Id { get; set; }

    public double TotalPrice { get; set; }

    public Guid ClientId { get; set; }

    public double CashbackSum { get; set; }

    public double TotalPriceBeforeCashback { get; set; }

    public virtual ICollection<ItemResponse>? Items { get; set; }
}
