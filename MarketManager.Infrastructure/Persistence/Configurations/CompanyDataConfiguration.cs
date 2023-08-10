using MarketManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketManager.Infrastructure.Persistence.Configurations
{
    public class CompanyDataConfiguration : IEntityTypeConfiguration<CompanyData>
    {
        public void Configure(EntityTypeBuilder<CompanyData> builder)
        {
            builder.Property(x => x.CompanyName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Data).IsRequired();
            builder.Property(x => x.Phone).HasMaxLength(12).IsRequired();
            builder.Property(x=>x.Logo).IsRequired();
            builder.Property(x=>x.Location).IsRequired();   
           
        }
    }
}
