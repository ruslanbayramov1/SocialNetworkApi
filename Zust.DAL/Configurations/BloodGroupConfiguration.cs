using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;

namespace Zust.DAL.Configurations;

public class BloodGroupConfiguration : IEntityTypeConfiguration<BloodGroup>
{
    public void Configure(EntityTypeBuilder<BloodGroup> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Name)
            .IsUnique();

        builder
             .Property(x => x.CreatedAt)
             .HasDefaultValueSql("GETDATE()");

        builder
            .Property(x => x.Name)
            .HasMaxLength(32)
            .IsRequired(true);

        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false);
    }
}
