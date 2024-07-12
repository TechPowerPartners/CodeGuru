using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotExtension.UI;

public static class UI
{
    public static InlineKeyboardMarkup GetInlineButtons(IEnumerable<string> buttons)
    {
        var inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();

        foreach (var button in buttons)
            inlineKeyboardButtons.Add([new(button) { CallbackData = button }]);

        return new InlineKeyboardMarkup(inlineKeyboardButtons);
    }

    public static InlineKeyboardMarkup GetInlineButtons(IEnumerable<(string, string)> buttons)
    {
        var inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();

        foreach (var button in buttons)
            inlineKeyboardButtons.Add([new(button.Item1) { CallbackData = button.Item2 }]);

        return new InlineKeyboardMarkup(inlineKeyboardButtons);
    }
}
