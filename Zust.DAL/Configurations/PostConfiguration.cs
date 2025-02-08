using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;

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
            .HasMaxLength(128)
            .IsRequired();

        builder
            .Property(x => x.ImageUrl)
            .HasMaxLength(128);

        builder
            .Property(x => x.VideoUrl)
            .HasMaxLength(128);
    }
}
