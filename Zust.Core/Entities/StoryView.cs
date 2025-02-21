using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

public class StoryView : BaseEntity
{
    public Guid ViewerUserId { get; set; }
    public User ViewerUser { get; set; }

    public Guid StoryId { get; set; }
    public Story Story { get; set; }
}
