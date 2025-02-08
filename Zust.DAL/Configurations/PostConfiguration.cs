using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;
using Zust.DAL.Settings;

namespace Zust.DAL.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .HasOne(x => x.PostedUser)
            .WithMany(u => u.Posts)
            .HasForeignKey(x => x.PostedUserId);

        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder
            .Property(x => x.Content)
            .HasMaxLength(PostSetting.ContentLength)
            .IsRequired();

        builder
            .Property(x => x.ImageUrl)
            .HasMaxLength(PostSetting.ImageUrlLength);

        builder
            .Property(x => x.VideoUrl)
            .HasMaxLength(PostSetting.VideoUrlLength);
    }
}
