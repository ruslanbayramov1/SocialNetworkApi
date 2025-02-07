using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Zust.BL.DTOs.BloodGroups;
using Zust.BL.DTOs.Genders;
using Zust.BL.DTOs.Languages;
using Zust.BL.DTOs.Occupations;
using Zust.BL.DTOs.RelationStatuses;
using Zust.BL.Exceptions.Common;
using Zust.BL.ExternalServices.Implements;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Services.Implements;
using Zust.BL.Services.Interfaces;
using Zust.Core.Enums;

namespace Zust.BL;

public static class ServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IGenderService, GenderService>();
        services.AddScoped<IBloodGroupService, BloodGroupService>();
        services.AddScoped<IOccupationService, OccupationService>();
        services.AddScoped<IRelationStatusService, RelationStatusService>();
        //services.AddScoped<IUserDataService, UserDataService>();
        services.AddScoped<IPostService, PostService>();
        //services.AddScoped<IAzureCloudBlobService, AzureBlobCloudService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();

        // external services
        //services.AddScoped<IUserClaimsService, UserClaimsService>();
        //services.AddScoped<IEmailService, EmailService>();

        return services;
    }

    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceRegistration));
        return services;
    }

    public static IApplicationBuilder UseSeedData(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            //var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            //var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var _genderService = scope.ServiceProvider.GetRequiredService<IGenderService>();
            var _languageService = scope.ServiceProvider.GetRequiredService<ILanguageService>();
            var _bloodGroupService = scope.ServiceProvider.GetRequiredService<IBloodGroupService>();
            var _relationStatusService = scope.ServiceProvider.GetRequiredService<IRelationStatusService>();
            var _occupationService = scope.ServiceProvider.GetRequiredService<IOccupationService>();

            //CreateRoles(_roleManager).Wait();
            //CreateAdmin(_userManager).Wait();

            CreateLanguages(_languageService).Wait();
            CreateGenders(_genderService).Wait();
            CreateOccupations(_occupationService).Wait();
            CreateBloodGroups(_bloodGroupService).Wait();
            CreateRelationStatuses(_relationStatusService).Wait();
        }
        return app;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining(typeof(ServiceRegistration));

        return services;
    }

    //private static async Task CreateRoles(RoleManager<IdentityRole> _roleManager)
    //{
    //    int res = await _roleManager.Roles.CountAsync();

    //    if (res == 0)
    //    {
    //        foreach (var role in Enum.GetValues(typeof(Roles)))
    //        {
    //            await _roleManager.CreateAsync(new IdentityRole(role.ToString()));
    //        }
    //    }
    //}

    //private static async Task CreateAdmin(UserManager<User> _userManager)
    //{
    //    if (!await _userManager.Users.AnyAsync(x => x.UserName == "admin"))
    //    {
    //        User user = new User
    //        {
    //            UserName = "admin",
    //            FirstName = "admin",
    //            LastName = "admin",
    //            Email = "admin@gmail.com",
    //        };
    //        user.EmailConfirmed = true;

    //        await _userManager.CreateAsync(user, "Admin_123");
    //        await _userManager.AddToRoleAsync(user, nameof(Roles.Admin));
    //    }
    //}

    private static async Task CreateLanguages(ILanguageService _languageService)
    {
        int res = await _languageService.GetCountAsync();

        if (res == 0)
        {
            foreach (var language in Enum.GetValues(typeof(Languages)))
            {
                await _languageService.CreateAsync(new LanguageCreateDto
                {
                    Name = language.ToString()
                });
            }
        }
    }

    private static async Task CreateGenders(IGenderService _genderService)
    {
        int res = await _genderService.GetCountAsync();

        if (res == 0)
        {
            foreach (var gender in Enum.GetValues(typeof(Genders)))
            {
                await _genderService.CreateAsync(new GenderCreateDto
                {
                    Name = gender.ToString()
                });
            }
        }
    }

    private static async Task CreateOccupations(IOccupationService _occupationService)
    {
        int res = await _occupationService.GetCountAsync();

        if (res == 0)
        {
            foreach (var occupation in Enum.GetValues(typeof(Occupations)))
            {
                await _occupationService.CreateAsync(new OccupationCreateDto
                {
                    Name = occupation.ToString()
                });
            }
        }
    }

    private static async Task CreateBloodGroups(IBloodGroupService _bloodGroupService)
    {
        int res = await _bloodGroupService.GetCountAsync();

        if (res == 0)
        {
            foreach (var bloodGroup in Enum.GetValues(typeof(BloodGroups)))
            {
                await _bloodGroupService.CreateAsync(new BloodGroupCreateDto
                {
                    Name = bloodGroup.ToString()
                });
            }
        }
    }

    private static async Task CreateRelationStatuses(IRelationStatusService _relationStatusService)
    {
        int res = await _relationStatusService.GetCountAsync();

        if (res == 0)
        {
            foreach (var relationStatus in Enum.GetValues(typeof(RelationStatuses)))
            {
                await _relationStatusService.CreateAsync(new RelationStatusCreateDto
                {
                    Name = relationStatus.ToString()
                });
            }
        }
    }
}