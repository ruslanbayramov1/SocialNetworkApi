using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;
using Zust.DAL.Settings;

namespace Zust.DAL.Configurations;

public class GenderConfiguration : IEntityTypeConfiguration<Gender>
{
    public void Configure(EntityTypeBuilder<Gender> builder)
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
            .HasMaxLength(GenderSetting.NameLength)
            .IsRequired(true);

        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false);
    }
}
