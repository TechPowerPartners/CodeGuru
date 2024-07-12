using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotExtension.FiniteStateMachine;
using TelegramBotExtension.Handling;
using TG.Bot.TelegramApi.Test;

namespace TG.Bot.TelegramApi;

internal static class Entry
{
    public static IServiceCollection ConfigureTelegramApi(this IServiceCollection services)
    {
        services
            .AddSingleton<BotService>()
            .AddTransient<IStorage, MemoryStorage>();

        var botClient = services.BuildServiceProvider().GetRequiredService<ITelegramBotClient>();

        botClient.SetMyCommandsAsync([
            new BotCommand() { Command = "/start", Description = "запустить бота" },
            new BotCommand() { Command = "/auth", Description = "авторизация" },
            new BotCommand() { Command = "/help", Description = "помощь" },
            ]);

        services.ConfigureTestServise();

        return services;
    }
}
