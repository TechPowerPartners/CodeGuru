using Telegram.Bot;
using TelegramBotExtension.Filters;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Types;
using TestingPlatform.Api.Contracts.Dto;
using TG.Bot.Enums;
using TG.Bot.TelegramApi.TestService.Views;

namespace TG.Bot.TelegramApi.TestService.Handlers;

internal class NextQuestionCallbackQueryHandler : CallbackQueryHandler
{
    [StateFilter(nameof(TestState.PassingTest))]
    [DataFilter("Следующий вопрос")]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        var lastMessageId = context.Update.CallbackQuery!.Message!.MessageId;
        var userData = await context.State.GetData();
        int messageId = (int)userData[nameof(messageId)];

        if (messageId != lastMessageId)
        {
            await QuestionView.ErrorAsync(context);
            return;
        }

        GetTestDto test = (GetTestDto)userData[nameof(test)];
        int index = (int)userData[nameof(index)];

        await SaveAnswerAsync(context, test.Questions.ElementAt(index));

        index++;

        if (index < test.Questions.Count)
        {
            await SendNextQuestionAsync(context, test, index);
            return;
        }
        await context.BotClient.EditMessageTextAsync(
            context.UserId,
            messageId,
            "Поздравляю!! тест пройден!!! "
            );
        await context.State.Clear();
    }

    public async Task SendNextQuestionAsync(TelegramContext context, GetTestDto test, int index)
    {
        await context.State.UpdateData(nameof(index), index);

        var currentQuestion = test.Questions.ElementAt(index);
        await context.State.UpdateData(nameof(currentQuestion), currentQuestion);

        await QuestionView.EditAsync(context, currentQuestion);

    }

    public Task SaveAnswerAsync(TelegramContext context, GetQuestionDto question)
    {
        return default!;
    }
}
