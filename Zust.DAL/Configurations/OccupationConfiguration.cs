using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;
using Zust.DAL.Settings;

namespace Zust.DAL.Configurations;

public class OccupationConfiguration : IEntityTypeConfiguration<Occupation>
{
    public void Configure(EntityTypeBuilder<Occupation> builder)
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
            .HasMaxLength(OccupationSetting.NameLength)
            .IsRequired(true);

        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false);
    }
}
