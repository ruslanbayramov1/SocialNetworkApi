using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zust.Core.Interfaces.Repositories;
using Zust.DAL.Contexts;
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
