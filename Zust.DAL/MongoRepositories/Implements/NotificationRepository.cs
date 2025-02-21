using Microsoft.Extensions.Options;
using Zust.Core.Enums;
using Zust.Core.Interfaces.MongoRepositories;
using Zust.Core.MongoEntities;
using Zust.DAL.Options;

namespace Zust.DAL.MongoRepositories.Implements;

public class NotificationRepository : MongoDbRepository<Notification>, INotificationRepository
{
    public NotificationRepository(IOptions<MongoOption> opt) : base(opt, MongoCollections.Notifications)
    {
    }
}
