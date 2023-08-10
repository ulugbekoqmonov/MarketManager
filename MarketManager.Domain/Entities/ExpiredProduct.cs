namespace MarketManager.Domain.Entities;

public class ExpiredProduct : BaseAuditableEntity
{
    public Guid PackageId { get; set; }
    public virtual Package Package { get; set; }
    public int Count { get; set; }
}
