using System.ComponentModel.DataAnnotations.Schema;

namespace MarketManager.Domain.Entities;

public class Order : BaseAuditableEntity
{
    [Column("TotalPrice")]
    private double _totalPrice;

    [NotMapped]
    public double TotalPrice
    {
        get
        {
            return _totalPrice;
        }
        private set
        {
            _totalPrice = TotalPriceBeforeCashback - cashBackSum;
        }
    }

    private double cashBackSum { get; set; } = 0;
    [NotMapped]
    public double CashbackSum
    {
        get
        {
            return cashBackSum;
        }
        set
        {
            if (this.Client?.CashbackSum >= value &&
                value > 0 &&
                TotalPriceBeforeCashback >= value)
            {
                    cashBackSum = value;
            }
            else cashBackSum = 0;
        }
    }
    public double TotalPriceBeforeCashback { get; set; }

    public Guid ClientId { get; set; }
    public virtual Client? Client { get; set; }

    public virtual ICollection<Item>? Items { get; set; }

}
