using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

public class Story : BaseEntity
{
    public DateTime ExpireDate { get; set; }
    public string? ImageUrl { get; set; }
    public string? Content { get; set; }
    public Guid StoryOwnerId { get; set; }
    public User StoryOwner { get; set; }

    public ICollection<StoryView> Views { get; set; } = new List<StoryView>();
    public ICollection<StoryLike> Likes { get; set; } = new List<StoryLike>();
}
