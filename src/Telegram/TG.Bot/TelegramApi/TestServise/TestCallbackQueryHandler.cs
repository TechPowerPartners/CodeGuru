using TG.Bot.Intagrations.BackendApi;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Api.Contracts.Tests.Dto;
using TelegramBotExtension.Types;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Filters;
using TelegramBotExtension.UI;

namespace TG.Bot.TelegramApi.Test;

internal class TestCallbackQueryHandler(IBackendApi _backendApi) : CallbackQueryHandler
{
    [StateFilter(nameof(States.SelectingTest))]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        var test = await GetTestAsync(context);

        if (!CheckValidTest(test))
        {
            await context.BotClient.AnswerCallbackQueryAsync(
                context.Update.CallbackQuery!.Id,
                "Ошибка! Поробуй выбрать другой тест😁",
                showAlert: true);
            return;
        }

        await SendMesssageAsync(context, test!);

        await context.State.SetState(nameof(States.StartTest));
        await context.State.UpdateData(nameof(test), test!);
    }

    private async Task<GetTestDto?> GetTestAsync(TelegramContext context)
    {
        if (!Guid.TryParse(context.Data, out Guid testId))
            return null;

        var response = await _backendApi.GetTestAsync(testId);
        return response.Content;
    }

    private async Task SendMesssageAsync(TelegramContext context, GetTestDto test)
    {
        var message = string.Format("<b>{0}</b>\n\n{1}", test!.Name, test.Description);

        await context.BotClient.SendTextMessageAsync(
            context.UserId,
            message,
            replyMarkup: UI.GetInlineButtons(["Начать тест"]),
            parseMode: ParseMode.Html);
    }

    private static bool CheckValidTest(GetTestDto? test)
       => test != null && test.Questions != null && test.Questions.Count != 0;
}
