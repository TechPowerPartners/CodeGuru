using Microsoft.Extensions.DependencyInjection;
using TelegramBotExtension.Handling;
using TG.Bot.TelegramApi.TestService.Handlers;

namespace TG.Bot.TelegramApi.TestService;

internal static class Entry
{
    public static IServiceCollection ConfigureTestService(this IServiceCollection services)
    {
        services
            .AddTransient<IUpdateTypeHandler, TestsCommandHandler>()
            .AddTransient<IUpdateTypeHandler, TestCallbackQueryHandler>()
            .AddTransient<IUpdateTypeHandler, StartTestCallbackQueryHandler>()
            .AddTransient<IUpdateTypeHandler, NextQuestionCallbackQueryHandler>()
            .AddTransient<IUpdateTypeHandler, SelectAnswerCallbackQueryHandler>();
        return services;
    }
}
