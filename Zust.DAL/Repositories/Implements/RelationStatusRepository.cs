using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class RelationStatusRepository : GenericRepository<RelationStatus>, IRelationStatusRepository
{
    public RelationStatusRepository(AppDbContext context) : base(context)
    {
    }
}
