using Microsoft.Extensions.DependencyInjection;
using TelegramBotExtension.Handling;
using TG.Bot.TelegramApi.TestServise.Handlers;

namespace TG.Bot.TelegramApi.Test;

internal static class Entry
{
    public static IServiceCollection ConfigureTestServise(this IServiceCollection services)
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
