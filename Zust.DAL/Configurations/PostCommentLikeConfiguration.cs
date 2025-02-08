using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;

namespace Zust.DAL.Configurations;

public class PostCommentLikeConfiguration : IEntityTypeConfiguration<PostCommentLike>
{
    public void Configure(EntityTypeBuilder<PostCommentLike> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .HasOne(x => x.PostComment)
            .WithMany(p => p.Likes)
            .HasForeignKey(x => x.PostCommentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder
            .HasOne(x => x.LikedUser)
            .WithMany(u => u.PostCommentLikes)
            .HasForeignKey(x => x.LikedUserId);
    }
}
