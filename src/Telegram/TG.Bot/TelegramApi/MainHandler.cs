using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using TG.Bot.Intagrations.BackendApi;
using TelegramBotDependencyInjection.Contracts;
using Api.Contracts.Tests.Requests;

namespace TG.Bot.TelegramApi;

class MainHandler(IBackendApi _backendApi) : IBotController
{
    private Random _random = new Random();

    public List<UpdateType> UpdateTypes { get; } = [UpdateType.Message];

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var userId = update.Message!.From!.Id;

        var testIds = await _backendApi.GetTestIdsAsync();
        
        var apiResponseTest = await _backendApi.GetTestAsync(testIds.Content.ToArray()[0]);
        var test = apiResponseTest.Content;
        await botClient.SendTextMessageAsync(userId, test.Name);
    }
}