using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;
using Zust.DAL.Settings;

namespace Zust.DAL.Configurations;

public class StoryConfiguration : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder
            .Property(x => x.ExpireDate)
            .HasDefaultValueSql("DATEADD(day, 1, GETDATE())");

        builder
            .Property(x => x.Content)
            .HasMaxLength(StorySetting.ContentLength);

        builder
            .Property(x => x.ImageUrl)
            .HasMaxLength(StorySetting.ImageUrlLength);
    }
}
