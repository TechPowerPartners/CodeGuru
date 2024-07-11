using Api.Contracts.Tests.Dto;
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

    public static InlineKeyboardMarkup GetInlineButtons(IEnumerable<(string, string)> buttons)
    {
        var inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();

        foreach (var button in buttons)
            inlineKeyboardButtons.Add([new(button.Item1) { CallbackData = button.Item2 }]);

        return new InlineKeyboardMarkup(inlineKeyboardButtons);
    }

    public static InlineKeyboardMarkup GetInlineAnswers(ICollection<GetAnswerDto> answers)
    {
        var inlineAnswers = answers.Select(a => (a.Text, a.Id.ToString())).ToList();

        var nextQuestionText = "Следующий вопрос";
        inlineAnswers.Add((nextQuestionText, nextQuestionText));

        return GetInlineButtons(inlineAnswers);
    }
}
