using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Zust.BL.Exceptions.Common;
using Zust.BL.Options;

namespace Zust.API;

public static class ServiceRegistration
{
    public static IServiceCollection AddOptionPatterns(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureOption>(configuration.GetSection(AzureOption.Position));
        services.Configure<SmtpOption>(configuration.GetSection(SmtpOption.Position));
        services.Configure<JwtOption>(configuration.GetSection(JwtOption.Position));
        services.Configure<AdminOption>(configuration.GetSection(AdminOption.Position));
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        JwtOption jwtOpt = new();
        jwtOpt.Issuer = configuration.GetSection(JwtOption.Position)[nameof(jwtOpt.Issuer)]!;
        jwtOpt.Audience = configuration.GetSection(JwtOption.Position)[nameof(jwtOpt.Audience)]!;
        jwtOpt.SecretKey = configuration.GetSection(JwtOption.Position)[nameof(jwtOpt.SecretKey)]!;

        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpt.SecretKey));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = signInKey,
                    ValidAudience = jwtOpt.Audience,
                    ValidIssuer = jwtOpt.Issuer,
                    ClockSkew = TimeSpan.Zero,
                };

                opt.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Authentication failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }

    public static IServiceCollection AddSwaggerGen(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Zust Api", Version = "1.0" });
        });

        return services;
    }

    public static IApplicationBuilder UseZustExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(handler =>
        {
            handler.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                Exception ex = feature!.Error;
                if (ex is IBaseException ibe)
                {
                    await context.Response.WriteAsJsonAsync(new
                    {
                        StatusCode = ibe.StatusCode,
                        Message = ibe.ErrorMessage
                    });
                }
                else
                {
                    await context.Response.WriteAsJsonAsync(new
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = ex.Message
                    });
                }
            });
        });

        return app;
    }
}
