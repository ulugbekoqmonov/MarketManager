namespace MarketManager.Domain.Entities;
public class Client : BaseAuditableEntity
{
    public string CardNumber { get; set; }
    public string? PhoneNumber { get; set; }
    public double CashbackSum { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}
  