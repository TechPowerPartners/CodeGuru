using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using System.Reflection;
using TelegramBotExtension.FiniteStateMachine;
using Telegram.Bot.Requests;
using Telegram.Bot.Types.Enums;
using TelegramBotExtension.Filters;
using TelegramBotExtension.Types;

namespace TelegramBotExtension.Handling;

public class BotService(
    IEnumerable<IUpdateTypeHandler> _handlers,
    ITelegramBotClient _botClient,
    IStorage _storage) : IUpdateHandler
{
    public Task StartBot()
    {
        State.Storage = _storage;
        _botClient.StartReceiving(this);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var handlers = GetHandlersByUpdateType(update.Type);

        foreach (var handler in handlers)
        {
            var methodInfo = handler.GetType().GetMethod(nameof(handler.HandleUpdateAsync));
            var context = handler.GetContext(botClient, update);

            if (await CheckFiltersAsync(methodInfo!, context))
            {
                await handler.HandleUpdateAsync(context);
                return;
            }
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        throw exception;
    }

    private static async Task<bool> CheckFiltersAsync(MethodInfo method, TelegramContext context)
    {
        var filters = method.GetCustomAttributes(false).OfType<FilterAttribute>();

        foreach (FilterAttribute filter in filters)
        {
            if (!await filter.Call(context))
                return false;
        }
        return true;
    }

    private IEnumerable<IUpdateTypeHandler> GetHandlersByUpdateType(UpdateType updateType)
    {
        return _handlers
            .Where(handler => handler
                                .GetType()
                                .GetCustomAttribute<HandlerAttribute>()!.UpdateType == updateType);
    }
}
