using Api.Contracts.Tests.Dto;
using Telegram.Bot;
using TelegramBotExtension.Filters;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Types;
using TG.Bot.Extensions;

namespace TG.Bot.TelegramApi.TestServise;

internal class NextQuestionCallbackQueryHandler : CallbackQueryHandler
{
    [StateFilter(nameof(States.PassingTest))]
    [DataFilter("Следующий вопрос")]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        var messageId = context.Update.CallbackQuery!.Message!.MessageId;

        var userData = await context.State.GetData();
        GetTestDto test = (GetTestDto)userData[nameof(test)];

        int index = (int)userData[nameof(index)];
        index++;

        if (index < test.Questions.Count)
        {
            await context.State.UpdateData(nameof(index), index);

            var currentQuestion = test.Questions.ElementAt(index);
            await context.State.UpdateData(nameof(currentQuestion), currentQuestion);

            await context.BotClient.EditMessageTextAsync(
                context.UserId,
                messageId,
                currentQuestion.Text,
                replyMarkup: currentQuestion.Answers.ToInlineKeyboardMarkup()
                );
            return;
        }
        await context.BotClient.EditMessageTextAsync(
            context.UserId,
            messageId,
            "Поздравляю!! тест пройден!!!"
            );
        await context.State.Clear();
    }
}
