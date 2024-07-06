using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TG.Bot.Intagrations.BackendApi;
using TelegramBotDependencyInjection.Contracts;

namespace TG.Bot.TelegramApi;

class MainHandler(IBackendApi _backend) : IBotController
{
    private Random _random = new Random();

    public List<UpdateType> UpdateTypes { get; } = [UpdateType.Message];

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var userId = update.Message!.From!.Id;
        var apiResponse = await _backend.GetAllTestsAsync();
        
        var messageTest = "apiResponse.ReasonPhrase == null";
        var messa = apiResponse.Headers;
        if (apiResponse.ReasonPhrase != null)
        {
        }  
        await botClient.SendTextMessageAsync(userId, messageTest);
    }
}