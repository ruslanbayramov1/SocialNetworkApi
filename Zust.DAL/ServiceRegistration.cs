using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zust.Core.Interfaces.MongoRepositories;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;
using Zust.DAL.MongoRepositories.Implements;
using Zust.DAL.Repositories.Implements;

namespace Zust.DAL;

public static class ServiceRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IGenderRepository, GenderRepository>();
        services.AddScoped<IBloodGroupRepository, BloodGroupRepository>();
        services.AddScoped<IOccupationRepository, OccupationRepository>();
        services.AddScoped<IRelationStatusRepository, RelationStatusRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostCommentRepository, PostCommentRepository>();
        services.AddScoped<IPostLikeRepository, PostLikeRepository>();
        services.AddScoped<IPostCommentLikeRepository, PostCommentLikeRepository>();
        services.AddScoped<IFollowRepository, FollowRepository>();
        services.AddScoped<IStoryRepository, StoryRepository>();
        services.AddScoped<IStoryLikeRepository, StoryLikeRepository>();
        services.AddScoped<IStoryViewRepository, StoryViewRepository>();

        // MongoDB repository
        services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration conf)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(conf.GetConnectionString("ZustAspRemote"));
        });
        return services;
    }
}
