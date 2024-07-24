using Microsoft.Extensions.DependencyInjection;
using TG.Bot.CacheServices.Base;
using TG.Bot.Common;

namespace TG.Bot.CacheServices;

internal static class Entry
{
    public static IServiceCollection ConfigureIntergrations(this IServiceCollection services)
    {
        services
            .AddTransient<ICachedBackendApiService, CachedBackendApiService>()
            .AddTransient<ICachedTestingPlatformApiService, CachedTestingPlatformApiService>();

        return services;
    }
}