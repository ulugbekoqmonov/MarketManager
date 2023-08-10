namespace MarketManager.Domain.Entities
{
    public class CompanyData : BaseAuditableEntity
    {
        public string CompanyName { get; set; }
        public string Logo { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public DateOnly Data { get; set; }
    }
}
