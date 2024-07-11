using TG.Bot.Intagrations.BackendApi;
using TG.Bot.Contracts;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using TG.Bot.Common;
using Api.Contracts.Tests.Dto;

namespace TG.Bot.TelegramApi.Test;

/// <summary>
/// Обработчик нажатия кнопки "Начать тест"
/// </summary>
/// <param name="_backendApi"></param>
internal class StartTestCallbackQueryController : IBotController
{
    public List<UpdateType> UpdateTypes => [UpdateType.CallbackQuery];

    public async Task HandleUpdateAsync(TelegramContext context)
    {
        var userId = context.Update.CallbackQuery!.From.Id;
        var data = context.Update.CallbackQuery!.Data;

        if (data == "Начать тест" && await context.CheckState(States.StartTest, userId))
        {
            var userData = await context.Storage.GetData(userId);
            GetTestDto test = (GetTestDto)userData[nameof(test)];
            var currentQuestion = test.Questions.First();

            await SendMessageAsync(context, userId, currentQuestion);

            await context.Storage.UpdateData(userId, nameof(currentQuestion), currentQuestion);
            await context.Storage.UpdateData(userId, "index", 0);
            await context.Storage.SetState(userId, States.PassingTest.ToString());
        }
    }

    private async Task SendMessageAsync(TelegramContext context, long userId, GetQuestionDto question)
    {
        await context.BotClient.SendTextMessageAsync(
            userId,
            question.Text,
            replyMarkup: UI.GetInlineAnswers(question.Answers),
            parseMode: ParseMode.Html);
    }
}