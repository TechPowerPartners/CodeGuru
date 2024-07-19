using TelegramBotExtension.Types;
using Telegram.Bot;
using TelegramBotExtension.UI;
using Api.Contracts.Tests.Dto;
using Telegram.Bot.Types.Enums;

namespace TG.Bot.TelegramApi.TestService.Views;

/// <summary>
/// Отображение теста
/// </summary>
/// <param name="_context"></param>
/// <param name="_test"></param>
internal class TestView
{
    /// <summary>
    /// Отправит сообщение в тг, в котором будет название и описание теста + кнопка
    /// "Начать тест"
    /// </summary>
    /// <returns></returns>
    public static async Task ShowAsync(TelegramContext context, GetTestDto test)
    {
        var message = string.Format("<b>{0}</b>\n\n{1}", test!.Name, test.Description);

        await context.BotClient.SendTextMessageAsync(
            context.UserId,
            message,
            replyMarkup: UI.GetInlineButtons(["Начать тест"]),
            parseMode: ParseMode.Html);
    }
}