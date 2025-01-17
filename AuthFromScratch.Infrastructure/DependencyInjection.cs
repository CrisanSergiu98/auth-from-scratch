using AuthFromScratch.Application.Common.Interfaces.Authentication;
using AuthFromScratch.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AuthFromScratch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastrcture(this IServiceCollection services)
    {       
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
}