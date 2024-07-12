using Api.Contracts.Tests.Dto;
using Telegram.Bot;
using TelegramBotExtension.Filters;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Types;
using TG.Bot.Extensions;

namespace TG.Bot.TelegramApi.TestServise;

internal class SelectAnswerCallbackQueryHandlerHandler : CallbackQueryHandler
{
    [StateFilter(nameof(States.PassingTest))]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        if (!ValidSelectAnswer(context, out Guid answerId))
        {
            await context.BotClient.AnswerCallbackQueryAsync(
                context.Update.CallbackQuery!.Id,
                "Ошибка!",
                showAlert: true);
            return;
        }
        await DecareSelectedAnswerAsync(context);
        await context.State.UpdateData(nameof(answerId), answerId);
    }

    private async Task DecareSelectedAnswerAsync(TelegramContext context)
    {
        var userData = await context.State.GetData();
        GetQuestionDto currentQuestion = (GetQuestionDto)userData[nameof(currentQuestion)];
        
        var answers = currentQuestion.Answers.MarkAnswer(context.Data);

        var messageId = context.Update.CallbackQuery!.Message!.MessageId;
        await context.BotClient.EditMessageReplyMarkupAsync(
           context.UserId,
           messageId,
           answers.ToInlineKeyboardMarkup()
           );
    }

    private bool ValidSelectAnswer(TelegramContext context, out Guid answerId)
    {
        var userId = context.Update.CallbackQuery!.From.Id;
        var userData = context.State.GetData().Result;
        var data = context.Update.CallbackQuery.Data;
        GetQuestionDto currentQuestion = (GetQuestionDto)userData[nameof(currentQuestion)];

        var parse = Guid.TryParse(data, out Guid id);
        var any = currentQuestion.Answers.Any(a => id == a.Id);
        answerId = id;
        return parse && any;
    }
}
