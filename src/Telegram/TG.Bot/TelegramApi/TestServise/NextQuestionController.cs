using Api.Contracts.Tests.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TG.Bot.Common;
using TG.Bot.Contracts;

namespace TG.Bot.TelegramApi.TestServise;

/// <summary>
/// Контроллер когда пользователь нажимает на кнопку "Следующий вопрос"
/// </summary>
internal class NextQuestionController : IBotController
{
    public List<UpdateType> UpdateTypes => [UpdateType.CallbackQuery];

    public async Task HandleUpdateAsync(TelegramContext context)
    {
        var userId = context.Update.CallbackQuery!.From.Id;
        var data = context.Update.CallbackQuery.Data;

        if (!await context.CheckState(States.PassingTest, userId))
        {
            await context.BotClient.AnswerCallbackQueryAsync(
                context.Update.CallbackQuery.Id,
                "Ошибка!",
                showAlert: true);
            return;
        }

        if (data != "Следующий вопрос") return;

        var userData = await context.Storage.GetData(userId);
        GetTestDto test = (GetTestDto)userData[nameof(test)];

        int index = (int)userData[nameof(index)];
        index++;

        if (index < test.Questions.Count)
        {
            await context.Storage.UpdateData(userId, nameof(index), index);

            var currentQuestion = test.Questions.ElementAt(index);
            await context.Storage.UpdateData(userId, nameof(currentQuestion), currentQuestion);

            await context.BotClient.EditMessageTextAsync(
                userId,
                context.Update.CallbackQuery.Message!.MessageId,
                currentQuestion.Text,
                replyMarkup: UI.GetInlineAnswers(currentQuestion.Answers)
                );

            return;
        }
        await context.BotClient.EditMessageTextAsync(
            userId,
            context.Update.CallbackQuery.Message!.MessageId,
            "Поздравляю!! тест пройден!!!"
            );
        await context.Storage.Clear(userId);
    }
}
