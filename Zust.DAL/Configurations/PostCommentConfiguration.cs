using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;

namespace Zust.DAL.Configurations;

public class PostCommentConfiguration : IEntityTypeConfiguration<PostComment>
{
    public void Configure(EntityTypeBuilder<PostComment> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .HasOne(x => x.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.ParentComment)
            .WithMany(x => x.Replies)
            .HasForeignKey(x => x.ParentCommentId);

        builder
            .HasOne(x => x.CommentedUser)
            .WithMany(x => x.PostComments)
            .HasForeignKey(x => x.CommentedUserId);

        builder
            .Property(x => x.Content)
            .HasMaxLength(128)
            .IsRequired(true);
    }
}
