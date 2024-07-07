using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TG.Bot.Common.FiniteStateMachine;
using TG.Bot.Contracts;
using TG.Bot.TelegramApi.Test;

namespace TG.Bot.TelegramApi;

internal static class Entry
{
    public static IServiceCollection ConfigureTelegramApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<BotService>()
            .AddTransient<IStorage, MemoryStorage>()
            .AddTransient<IBotController, TestCallbackQuery>();

        return services;
    }
}
