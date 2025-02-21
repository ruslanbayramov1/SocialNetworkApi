using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;

namespace Zust.DAL.Configurations;

public class StoryLikeConfiguration : IEntityTypeConfiguration<StoryLike>
{
    public void Configure(EntityTypeBuilder<StoryLike> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder
            .HasOne(x => x.Story)
            .WithMany(s => s.Likes)
            .HasForeignKey(x => x.StoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.LikedUser)
            .WithMany(u => u.StoryLikes)
            .HasForeignKey(x => x.LikedUserId);
    }
}
