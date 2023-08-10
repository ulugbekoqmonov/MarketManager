using MarketManager.Application.UseCases.Products.Response;
using MarketManager.Domain.Entities;

namespace MarketManager.Application.Common.Abstraction
{
    public class ExpiredProductResponce
    {
        public Guid Id { get; set; }
        public Guid PackageId { get; set; }
        public int Count { get; set; }
        public ProductResponse Product { get; set; }
        public double IncomingPrice { get; set; }
        public double SalePrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifyBy { get; set; }
    }
}
