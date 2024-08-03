using TG.Bot.Intagrations.BackendApi;
using TelegramBotExtension.Types;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Filters;
using TG.Bot.TelegramApi.TestService.Views;
using Telegram.Bot;
using TG.Bot.Enums;
using TestingPlatform.Api.Contracts.Dto;
using TG.Bot.Intagrations.TestingPlatformApi;

namespace TG.Bot.TelegramApi.TestService.Handlers;

/// <summary>
/// Обработчик нажатия кнопки на стадии выбора теста
/// </summary>
/// <param name="_testingPlatformApi"></param>
internal class TestCallbackQueryHandler(ITestingPlatformApi _testingPlatformApi) : CallbackQueryHandler
{
    [StateFilter(nameof(TestState.SelectingTest))]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        var test = await GetTestAsync(context);

        if (!IsValidTest(test))
        {
            await context.BotClient.AnswerCallbackQueryAsync(
                context.Update.CallbackQuery!.Id,
                "Ошибка! Поробуй выбрать другой тест😁",
                showAlert: true);
            return;
        }
        await TestView.ShowAsync(context, test!);

        await context.State.SetState(nameof(TestState.StartTest));
        await context.State.UpdateData(nameof(test), test!);
    }

    private async Task<GetTestDto?> GetTestAsync(TelegramContext context)
    {
        if (!Guid.TryParse(context.Data, out Guid testId))
            return null;

        var response = await _testingPlatformApi.GetTestAsync(testId);

        if (!response.IsSuccessStatusCode) return null;
        return response.Content;
    }

    private static bool IsValidTest(GetTestDto? test)
       => test != null && test.Questions != null && test.Questions.Count != 0;
}
