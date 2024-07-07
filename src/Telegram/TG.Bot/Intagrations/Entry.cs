using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TG.Bot.Intagrations.BackendApi;

namespace TG.Bot.Intagrations;

internal static class Entry
{
    public static IServiceCollection ConfigureIntergrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureBackendApiIntergration(configuration);

        return services;
    }
}