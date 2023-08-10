using MarketManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketManager.Infrastructure.Persistence.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.Property(supplier => supplier.Name)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(supplier => supplier.Phone)
            .HasMaxLength(20)
            .IsRequired();
        }
    }
}
