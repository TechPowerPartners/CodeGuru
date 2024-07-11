using Microsoft.Extensions.DependencyInjection;
using TG.Bot.Contracts;
using TG.Bot.TelegramApi.TestServise;

namespace TG.Bot.TelegramApi.Test;

internal static class Entry
{
    public static IServiceCollection ConfigureTestServise(this IServiceCollection services)
    {
        services
            .AddTransient<IBotController, TestsCommandController>()
            .AddTransient<IBotController, StartTestCallbackQueryController>()
            .AddTransient<IBotController, TestCallbackQueryController>()
            .AddTransient<IBotController, SelectAnswerController>();

        return services;
    }
}
