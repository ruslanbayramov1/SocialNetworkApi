using Zust.Core.Entities.Common;

namespace Zust.Core.Entities;

public class BloodGroup : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<User> Users { get; set; } = new List<User>();
}
