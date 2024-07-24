using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace TG.Bot.Intagrations.TestingPlatformApi;

internal static class Entry
{
    public static IServiceCollection ConfigureTestingPlatformApiIntagration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRefitClient<ITestingPlatformApi>().ConfigureHttpClient(
            client => client.BaseAddress = new(configuration.GetValue<string>("ApiTestingPlatformUrl")!)
            );

        return services;
    }

}
