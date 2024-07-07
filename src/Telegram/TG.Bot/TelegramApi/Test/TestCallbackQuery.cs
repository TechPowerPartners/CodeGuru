using TG.Bot.Intagrations.BackendApi;
using TG.Bot.Contracts;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using TG.Bot.Common;

namespace TG.Bot.TelegramApi.Test;

/// <summary>
/// Обработчик нажатия встроенной клавиатуры выбранного теста
/// </summary>
/// <param name="_backendApi"></param>
internal class TestCallbackQuery(IBackendApi _backendApi) : IBotController
{
    public List<UpdateType> UpdateTypes => [UpdateType.CallbackQuery];

    public async Task HandleUpdateAsync(TelegramContext context)
    {


        var testIds = await _backendApi.GetTestIdsAsync();
        var apiResponseTest = await _backendApi.GetTestAsync(testIds.Content!.ToArray()[0]);
        var getTestDto = apiResponseTest.Content;

        await context.BotClient.SendTextMessageAsync(
            context.Update.Message.From!.Id,
            string.Format("<b>{0}</b>\n\n{1}", getTestDto!.Name, getTestDto.Description),
            replyMarkup: UI.GetInlineButtons(["Начать тест"]),
            parseMode: ParseMode.Html);
    }
}
