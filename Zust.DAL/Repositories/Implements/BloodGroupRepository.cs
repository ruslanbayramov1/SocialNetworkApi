using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class BloodGroupRepository : GenericRepository<BloodGroup>, IBloodGroupRepository
{
    public BloodGroupRepository(AppDbContext context) : base(context)
    {
    }
}
