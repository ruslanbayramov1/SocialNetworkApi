using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

public class StoryLike : BaseEntity
{
    public Guid LikedUserId { get; set; }
    public User LikedUser { get; set; }

    public Guid StoryId { get; set; }
    public Story Story { get; set; }
}
