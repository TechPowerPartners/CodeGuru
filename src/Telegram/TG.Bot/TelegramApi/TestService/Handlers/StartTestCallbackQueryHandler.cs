﻿using TelegramBotExtension.Handling;
using TelegramBotExtension.Types;
using TelegramBotExtension.Filters;
using TG.Bot.TelegramApi.TestService.Views;
using TG.Bot.Enums;
using TestingPlatform.Api.Contracts.Dto;

namespace TG.Bot.TelegramApi.TestService.Handlers;

/// <summary>
/// Обработчик нажатия кнопки "Начать тест", пользователь начинает проходить тест
/// </summary>
internal class StartTestCallbackQueryHandler : CallbackQueryHandler
{
    [DataFilter("Начать тест")]
    [StateFilter(nameof(TestState.StartTest))]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        var userData = await context.State.GetData();
        GetTestDto test = (GetTestDto)userData[nameof(test)];
        var currentQuestion = test.Questions.First();

        var message = await QuestionView.ShowAsync(context, currentQuestion);

        var messageId = message.MessageId;

        int questionIndex = 0;

        await context.State.UpdateData(new() {
            { nameof(currentQuestion), currentQuestion },
            { nameof(questionIndex), questionIndex },
            { nameof(messageId), messageId }
        });
        await context.State.SetState(nameof(TestState.PassingTest));
    }
}