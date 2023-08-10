namespace MarketManager.Domain.Entities
{
    public class Cashback : BaseAuditableEntity
    {
        public sbyte CashbackPercent { get; set; }
    }
}
