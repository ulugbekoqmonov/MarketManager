using MarketManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketManager.Infrastructure.Persistence.Configurations
{
    public class ExpiredProductConfiguration : IEntityTypeConfiguration<ExpiredProduct>
    {
        public void Configure(EntityTypeBuilder<ExpiredProduct> builder)
        {
            builder.Property(x => x.PackageId).IsRequired();
            builder.Property(x => x.Count).IsRequired();
        }
    }
}
