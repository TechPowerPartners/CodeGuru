using TG.Bot.Intagrations.BackendApi;
using TG.Bot.Contracts;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using TG.Bot.Common;
using Api.Contracts.Tests.Dto;

namespace TG.Bot.TelegramApi.Test;

/// <summary>
/// Обработчик нажатия кнопки для вывода сообщение о выбранном тесте
/// </summary>
/// <param name="_backendApi"></param>
internal class TestCallbackQueryController(IBackendApi _backendApi) : IBotController
{
    public List<UpdateType> UpdateTypes => [UpdateType.CallbackQuery];

    public async Task HandleUpdateAsync(TelegramContext context)
    {
        var userId = context.Update.CallbackQuery!.From.Id;
        
        if (!await context.CheckState(States.SelectingTest, userId))
            return;

        var test = await GetTestAsync(context);

        if (!CheckValidTest(test))
        {
            await context.BotClient.AnswerCallbackQueryAsync(
                context.Update.CallbackQuery.Id,
                "Ошибка! Поробуй выбрать другой тест😁",
                showAlert: true);
            return;
        }

        await SendMesssageAsync(context, userId, test!);

        await context.Storage.SetState(userId, States.StartTest.ToString());
        await context.Storage.UpdateData(userId, nameof(test), test!);
    }

    private async Task<GetTestDto?> GetTestAsync(TelegramContext context)
    {
        if (!Guid.TryParse(context.Update.CallbackQuery!.Data!, out Guid testId))
            return null;

        var response = await _backendApi.GetTestAsync(testId);
        return response.Content;
    }

    private async Task SendMesssageAsync(TelegramContext context, long userId, GetTestDto test)
    {
        var message = string.Format("<b>{0}</b>\n\n{1}", test!.Name, test.Description);

        await context.BotClient.SendTextMessageAsync(
            userId,
            message,
            replyMarkup: UI.GetInlineButtons(["Начать тест"]),
            parseMode: ParseMode.Html);
    }

    private static bool CheckValidTest(GetTestDto? test)
       => test != null && test.Questions != null && test.Questions.Count != 0;
}
