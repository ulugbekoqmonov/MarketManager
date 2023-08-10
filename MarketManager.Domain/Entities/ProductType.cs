namespace MarketManager.Domain.Entities;

public class ProductType : BaseAuditableEntity
{
    public string Name { get; set; }
    public string? Picture { get; set; }

    public virtual ICollection<Product> Products { get; set; }
}
