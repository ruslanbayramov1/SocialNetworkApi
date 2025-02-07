using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class OccupationRepository : GenericRepository<Occupation>, IOccupationRepository
{
    public OccupationRepository(AppDbContext context) : base(context)
    {
    }
}
