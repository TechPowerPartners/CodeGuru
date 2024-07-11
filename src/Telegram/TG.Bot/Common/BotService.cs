using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TG.Bot.Common.FiniteStateMachine;
using TG.Bot.Contracts;

namespace TG.Bot.Common;

internal class BotService(
    IEnumerable<IBotController> _botControllers,
    ITelegramBotClient _botClient,
    IStorage _storage) : IUpdateHandler
{
    public Task StartBot()
    {
        _botClient.StartReceiving(this);
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var targetControllers = _botControllers.Where(controller
           => controller.UpdateTypes.Contains(update.Type));

        Parallel.ForEach(targetControllers, (controller) =>
        {
            controller.HandleUpdateAsync(new(botClient, update, cancellationToken, _storage));
        });
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
}
