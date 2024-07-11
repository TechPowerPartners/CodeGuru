using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Api.Contracts.Tests.Dto;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Types;
using TelegramBotExtension.Filters;
using TG.Bot.Extensions;

namespace TG.Bot.TelegramApi.Test;

internal class StartTestCallbackQueryHandler : CallbackQueryHandler
{
    [DataFilter("Начать тест")]
    [StateFilter(nameof(States.StartTest))]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        var userData = await context.State.GetData();
        GetTestDto test = (GetTestDto)userData[nameof(test)];
        var currentQuestion = test.Questions.First();

        await SendMessageAsync(context, context.UserId, currentQuestion);

        int index = 0;

        await context.State.UpdateData(new() {
            { nameof(currentQuestion), currentQuestion },
            { nameof(index), index }
        });
        await context.State.SetState(nameof(States.PassingTest));
    }

    private async Task SendMessageAsync(TelegramContext context, long userId, GetQuestionDto question)
    {
        await context.BotClient.SendTextMessageAsync(
            userId,
            question.Text,
            replyMarkup: question.Answers.ToInlineKeyboardMarkup(),
            parseMode: ParseMode.Html);
    }
}