using Microsoft.EntityFrameworkCore;
using Zust.Core.Entities;
using Zust.Core.Entities.Common;

namespace Zust.DAL.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions opt) : base(opt)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(builder);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.Now;
            }
            else if (entry.State == EntityState.Deleted)
            { 
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<BloodGroup> BloodGroups { get; set; }
    public DbSet<Gender> Genders { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Occupation> Occupations { get; set; }
    public DbSet<RelationStatus> RelationStatuses { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<PostCommentLike> PostCommentLikes { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public DbSet<Story> Stories { get; set; }
    public DbSet<StoryView> StoryViews { get; set; }
    public DbSet<StoryLike> StoryLikes { get; set; }
}
