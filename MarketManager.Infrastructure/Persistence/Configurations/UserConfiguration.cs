using MarketManager.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketManager.Infrastructure.Persistence.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.Username)
            .IsRequired()
            .HasMaxLength(20);
        builder.HasIndex(user => user.Username).IsUnique();
        builder.Property(user => user.Phone)
        .HasMaxLength(20)
        .IsRequired();
        builder.Property(user => user.FullName)
            .IsRequired()
            .HasMaxLength(40);
        builder.Property(user => user.Password).IsRequired();


    }
}
