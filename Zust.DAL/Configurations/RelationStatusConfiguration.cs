using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;
using Zust.DAL.Settings;

namespace Zust.DAL.Configurations;

public class RelationStatusConfiguration : IEntityTypeConfiguration<RelationStatus>
{
    public void Configure(EntityTypeBuilder<RelationStatus> builder)
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
            .HasMaxLength(RelationStatusSetting.NameLength)
            .IsRequired(true);

        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false);
    }
}
