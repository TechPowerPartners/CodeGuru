using Api.Contracts.Tests.Dto;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotExtension.UI;

namespace TG.Bot.Extensions;

public static class AnswerExtension
{
    public static InlineKeyboardMarkup ToInlineKeyboardMarkup(this ICollection<GetAnswerDto> answers)
    {
        var inlineAnswers = answers.Select(a => (a.Text, a.Id.ToString())).ToList();

        var nextQuestionText = "Следующий вопрос";
        inlineAnswers.Add((nextQuestionText, nextQuestionText));

        return UI.GetInlineButtons(inlineAnswers);
    }

    public static ICollection<GetAnswerDto> MarkAnswer(this ICollection<GetAnswerDto> answers, string data)
    {
        return answers.Select(answer =>
        {
            if (!Guid.TryParse(data, out Guid answerId))
                return answer;

            if (answer.Id == answerId)
                answer = new GetAnswerDto { Id = answer.Id, Text = "☑️" + answer.Text };
            return answer;
        }).ToArray();
    }
}
