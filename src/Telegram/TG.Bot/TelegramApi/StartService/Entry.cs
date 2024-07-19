using Microsoft.Extensions.DependencyInjection;
using TelegramBotExtension.Handling;

namespace TG.Bot.TelegramApi.StartService;

internal static class Entry
{
    public static IServiceCollection ConfigureStartService(this IServiceCollection services)
    {
        services
            .AddTransient<IUpdateTypeHandler, StartCommandHandler>();
        return services;
    }
}
