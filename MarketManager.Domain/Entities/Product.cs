using MarketManager.Domain.States;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketManager.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Barcode { get; set; }
    public double ExistCount { get; set; }
    public double SalePrice { get; private set; }
    public MeasureTypes MeasureType { get; set; }
    public string Description { get; set; }
    public string? Picture { get; set; }
    public Guid ProductTypeId { get; set; }
    public virtual ProductType ProductType { get; set; }

    public virtual ICollection<Package> Packages { get; set; }
    public virtual ICollection<Item> Items { get; set; }

    [Column("sale_price_before_discount")]
    private double salePriceBeforeDiscount { get; set; }
    private double discount { get; set; }

    /// <summary>
    /// Input value amount in Percentage(%)
    /// </summary>
    [NotMapped]
    public double Discount
    {
        get
        {
            return discount;
        }
        set
        {
            if (value > 0)
            {
                discount = value;
                SalePrice = salePriceBeforeDiscount * (1 - (discount / 100));
            }
        }
    }
}
