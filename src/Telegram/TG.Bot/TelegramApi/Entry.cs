﻿using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotExtension.FiniteStateMachine;
using TelegramBotExtension.Handling;
using TG.Bot.TelegramApi.AuthService;
using TG.Bot.TelegramApi.HelpService;
using TG.Bot.TelegramApi.StartService;
using TG.Bot.TelegramApi.TestService;

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
            new BotCommand() { Command = "/start", Description = "Запустить бота" },
            new BotCommand() { Command = "/tests", Description = "Показать тесты" },
            new BotCommand() { Command = "/auth", Description = "Aвторизация" },
            new BotCommand() { Command = "/help", Description = "Помощь" },
            ]);

        services
            .ConfigureStartService()
            .ConfigureHelpService()
            .ConfigureAuthService()
            .ConfigureTestService();

        return services;
    }
}
