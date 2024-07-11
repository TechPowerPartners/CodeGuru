using Api.Contracts.Tests.Dto;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TG.Bot.Common;
using TG.Bot.Contracts;
using TG.Bot.Intagrations.BackendApi;

namespace TG.Bot.TelegramApi.Test;

/// <summary>
/// Команда на отображение всех названий тестов для дальнейшего выбора пользователем
/// </summary>
/// <param name="_backendApi"></param>
internal class TestsCommandController(IBackendApi _backendApi) : IBotController
{
    public List<UpdateType> UpdateTypes => [UpdateType.Message];

    public async Task HandleUpdateAsync(TelegramContext context)
    {
        if (context.Update.Message!.Text != "/tests") return;

        var userId = context.Update.Message.From!.Id;

        var response = await _backendApi.GetTestIdsAsync();
        var testNamesAndIds = response.Content!;

        if (testNamesAndIds.Count == 0)
        {
            await context.BotClient.SendTextMessageAsync(
                userId,
                "Тестов пока что нет, попробуйте позже... :(");
            return;
        }

        await SendMessageAsync(context, testNamesAndIds);

        await context.Storage.Clear(userId);
        await context.Storage.SetState(userId, States.SelectingTest.ToString());
    }

    private async Task SendMessageAsync(TelegramContext context, ICollection<GetTestNameAndIdDto> testNamesAndIds)
    {
        var testNamesAndIdsDict = testNamesAndIds.Select(a => (a.Name, a.Id.ToString()));

        await context.BotClient.SendTextMessageAsync(
            context.Update.Message!.From!.Id,
            "Выбери тест который будешь проходить",
            replyMarkup: UI.GetInlineButtons(testNamesAndIdsDict)
            );
    }
}