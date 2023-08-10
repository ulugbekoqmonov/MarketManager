using MarketManager.Domain.States;

namespace MarketManager.Application.UseCases.Products.Response
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public Guid ProductTypeId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public double ExistCount { get; set; }
        public double SalePrice { get; private set; }
        public string Description { get; set; }
        public string? Picture { get; set; }
        public MeasureTypes MeasureType { get; set; }
        public double Discount { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifyBy { get; set; }
    }
}
