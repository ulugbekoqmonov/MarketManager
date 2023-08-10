namespace MarketManager.Domain.Entities;

public class Item : BaseAuditableEntity
{
    public double Amount { get; set; }
    public double TotalPrice { get; set; }

    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public Guid OrderId { get; set; }
    public virtual Order Order { get; set; }
}
