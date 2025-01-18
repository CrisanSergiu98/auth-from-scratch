using AuthFromScratch.Application.Common.Interfaces.Authentication;
using AuthFromScratch.Application.Common.Interfaces.Services;
using AuthFromScratch.Infrastructure.Authentication;
using AuthFromScratch.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AuthFromScratch.Application.Common.Interfaces.Persistence;
using AuthFromScratch.Infrastructure.Persistence;

namespace AuthFromScratch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastrcture(
        this IServiceCollection services, 
        ConfigurationManager configuration)
    {       
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IUserRepository, InMemoryUserRepository>();
        
        return services;
    }
}