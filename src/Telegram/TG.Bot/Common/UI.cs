using Telegram.Bot.Types.ReplyMarkups;

namespace TG.Bot.Common;

internal static class UI
{
    public static InlineKeyboardMarkup GetInlineButtons(IEnumerable<string> buttons)
    {
        var inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();

        foreach (var button in buttons)
            inlineKeyboardButtons.Add([new(button) { CallbackData = button }]);

        return new InlineKeyboardMarkup(inlineKeyboardButtons);
    }
}
