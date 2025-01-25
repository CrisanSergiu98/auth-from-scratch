using AuthFromScratch.Api.Common.Errors;
using AuthFromScratch.Api.Common.Mapping;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AuthFromScratch.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {      
        services.AddOpenApi();
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, AuthFromScratchProblemDetailsFactory>();       
        services.AddMappings();
        return services;
    }
}