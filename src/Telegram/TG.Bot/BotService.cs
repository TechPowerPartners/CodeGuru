using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotDependencyInjection.Contracts;

namespace TelegramBotDependencyInjection;

internal class BotService : IUpdateHandler
{
    private readonly IEnumerable<IBotController> _botControllers;
    private readonly ITelegramBotClient _botClient;

    public BotService(IEnumerable<IBotController> botControllers, ITelegramBotClient botClient)
    {
        _botControllers = botControllers;
        _botClient = botClient;
    }

    public Task StartBot()
    {
        _botClient.StartReceiving(this);
        return Task.CompletedTask;
    }

    public async Task GetMeAsync()
    {
        var me = await _botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
    }


    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var targetControllers = _botControllers.Where(controller
            => controller.UpdateTypes.Contains(update.Type));
        
        Parallel.ForEach(targetControllers, (controller) =>
        {
            controller.HandleUpdateAsync(botClient, update, cancellationToken);
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
        return Task.CompletedTask;
    }
}