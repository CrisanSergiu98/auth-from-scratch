using AuthFromScratch.Application.Common.Interfaces.Authentication;
using AuthFromScratch.Application.Common.Interfaces.Services;
using AuthFromScratch.Infrastructure.Authentication;
using AuthFromScratch.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AuthFromScratch.Application.Common.Interfaces.Persistence;
using AuthFromScratch.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

namespace AuthFromScratch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastrcture(
        this IServiceCollection services, 
        ConfigurationManager configuration)
    {       
        services.AddAuth(configuration);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<IUserRepository, InMemoryUserRepository>();
        
        return services;
    }

    public static IServiceCollection AddAuth(
        this IServiceCollection services, 
        ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });

        return services;
    }
}