namespace MarketManager.Application.UseCases.CompanysData.Responce
{
    public class CompanyDataResponce
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string Logo { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public DateOnly Data { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifyBy { get; set; }
    }
}
