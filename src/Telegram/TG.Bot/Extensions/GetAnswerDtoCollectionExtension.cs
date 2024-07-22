using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotExtension.UI;
using TestingPlatform.Api.Contracts.Dto;

namespace TG.Bot.Extensions;

public static class GetAnswerDtoCollectionExtension
{
    public static InlineKeyboardMarkup ToInlineKeyboardMarkup(this ICollection<GetAnswerDto> answers)
    {
        var inlineAnswers = answers.Select(a => (a.Text, a.Id.ToString())).ToList();

        var nextQuestionText = "Следующий вопрос";
        inlineAnswers.Add((nextQuestionText, nextQuestionText));

        return UI.GetInlineButtons(inlineAnswers);
    }

    public static ICollection<GetAnswerDto> MarkAnswer(this ICollection<GetAnswerDto> answers, Guid answerId)
    {
        return answers.Select(answer =>
        {
            if (answer.Id == answerId)
                answer = new GetAnswerDto { Id = answer.Id, Text = "☑️" + answer.Text };
            return answer;
        }).ToArray();
    }
}
