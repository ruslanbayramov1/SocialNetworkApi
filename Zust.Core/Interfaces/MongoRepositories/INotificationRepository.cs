using Zust.Core.MongoEntities;

namespace Zust.Core.Interfaces.MongoRepositories;

public interface INotificationRepository : IMongoDbRepository<Notification>
{
}
