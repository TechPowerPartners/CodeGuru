using Api.Contracts.Tests.Dto;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBotExtension.Filters;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Types;
using TelegramBotExtension.UI;
using TG.Bot.Intagrations.BackendApi;

namespace TG.Bot.TelegramApi.Test;

internal class TestsCommandHandler(IBackendApi _backendApi) : MessageHandler
{
    [Command("tests")]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        var response = await _backendApi.GetTestIdsAsync();
        var testNamesAndIds = response.Content!;

        if (testNamesAndIds.Count == 0)
        {
            await context.BotClient.SendTextMessageAsync(
                context.UserId,
                "Тестов пока что нет, попробуйте позже... :(");
            return;
        }

        await SendMessageAsync(context, testNamesAndIds);

        await context.State.Clear();
        await context.State.SetState(nameof(States.SelectingTest));
    }

    private async Task SendMessageAsync(TelegramContext context, ICollection<GetTestNameAndIdDto> testNamesAndIds)
    {
        var testNamesAndIdsDict = testNamesAndIds.Select(a => (a.Name, a.Id.ToString()));

        await context.BotClient.SendTextMessageAsync(
            context.UserId,
            "Выбери тест который будешь проходить",
            replyMarkup: UI.GetInlineButtons(testNamesAndIdsDict)
            );
    }
}
