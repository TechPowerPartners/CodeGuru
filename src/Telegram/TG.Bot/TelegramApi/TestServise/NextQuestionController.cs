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
        GetQuestionDto currentQuestion = (GetQuestionDto)userData[nameof(currentQuestion)];

        
    }
}
