using TelegramBotExtension.Types;
using Telegram.Bot;
using TelegramBotExtension.UI;
using Api.Contracts.Tests.Dto;

namespace TG.Bot.TelegramApi.TestServise.Views;

/// <summary>
/// Отображение тестов
/// </summary>
/// <param name="_context"></param>
/// <param name="_testNamesAndIds"></param>
internal class TestsView
{
    /// <summary>
    /// Отправит сообщение в тг, в котором будут тесты в виде кнопок.
    /// </summary>
    /// <returns></returns>
    public static async Task ShowAsync(TelegramContext context, ICollection<GetTestNameAndIdDto> testNamesAndIds)
    {
        var testNamesAndIdsDict = testNamesAndIds.Select(a => (a.Name, a.Id.ToString()));

        await context.BotClient.SendTextMessageAsync(
            context.UserId,
            "Выбери тест который будешь проходить",
            replyMarkup: UI.GetInlineButtons(testNamesAndIdsDict)
            );
    }
}
