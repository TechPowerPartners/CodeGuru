using Api.Contracts.Tests.Dto;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TG.Bot.Common;
using TG.Bot.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace TG.Bot.TelegramApi.TestServise;

/// <summary>
/// Контроллер когда пользователь выбрал ответ на вопрос
/// </summary>
internal class SelectAnswerController : IBotController
{
    public List<UpdateType> UpdateTypes => [UpdateType.CallbackQuery];

    public async Task HandleUpdateAsync(TelegramContext context)
    {
        var userId = context.Update.CallbackQuery!.From.Id;
        var data = context.Update.CallbackQuery.Data;

        if (!await context.CheckState(States.PassingTest, userId))
            return;

        if (data == "Следующий вопрос") return;

        if(!ValidSelectAnswer(context, out Guid answerId))
        {
            await context.BotClient.AnswerCallbackQueryAsync(
                context.Update.CallbackQuery.Id,
                "Ошибка!",
                showAlert: true);
            return;
        }

        await DecareSelectedAnswerAsync(context, userId, data!);
        await context.Storage.UpdateData(userId, nameof(answerId), answerId);
    }

    private async Task DecareSelectedAnswerAsync(TelegramContext context, long userId, string data)
    {
        var userData = await context.Storage.GetData(userId);
        GetQuestionDto currentQuestion = (GetQuestionDto)userData[nameof(currentQuestion)];
        
        var answers = MarkAnswer(currentQuestion.Answers, data);

        var messageId = context.Update.CallbackQuery!.Message!.MessageId;
        await context.BotClient.EditMessageReplyMarkupAsync(
           userId,
           messageId,
           UI.GetInlineAnswers(answers)
           );
    }

    private bool ValidSelectAnswer(TelegramContext context, out Guid answerId)
    {
        var userId = context.Update.CallbackQuery!.From.Id;
        var userData = context.Storage.GetData(userId).Result;
        var data = context.Update.CallbackQuery.Data;
        GetQuestionDto currentQuestion = (GetQuestionDto)userData[nameof(currentQuestion)];

        var parse = Guid.TryParse(data, out Guid id);
        var any = currentQuestion.Answers.Any(a => id == a.Id);
        answerId = id;
        return parse && any;
    }

    private  static ICollection<GetAnswerDto> MarkAnswer(ICollection<GetAnswerDto> answers, string data)
    {
        return answers.Select(answer =>
        {
            if (answer.Id == Guid.Parse(data))
                answer = new GetAnswerDto { Id = answer.Id, Text = "☑️" + answer.Text };
            return answer;
        }).ToArray();
    }
}
