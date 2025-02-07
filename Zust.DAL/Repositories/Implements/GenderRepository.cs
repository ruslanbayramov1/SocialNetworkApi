using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;

namespace Zust.DAL.Repositories.Implements;

public class GenderRepository : GenericRepository<Gender>, IGenderRepository
{
    public GenderRepository(AppDbContext context) : base(context)
    {
    }
}
