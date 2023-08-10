namespace MarketManager.Domain.Entities;

public class Supplier : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public virtual ICollection<Package> Packages { get; set; }
}
