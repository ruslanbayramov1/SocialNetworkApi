using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;

namespace Zust.DAL.Configurations;

public class StoryViewConfiguration : IEntityTypeConfiguration<StoryView>
{
    public void Configure(EntityTypeBuilder<StoryView> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder
            .HasOne(x => x.Story)
            .WithMany(s => s.Views)
            .HasForeignKey(x => x.StoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.ViewerUser)
            .WithMany(u => u.StoryViews)
            .HasForeignKey(x => x.ViewerUserId);
    }
}
