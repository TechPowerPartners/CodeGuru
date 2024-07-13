using TG.Bot.Intagrations.BackendApi;
using Api.Contracts.Tests.Dto;
using TelegramBotExtension.Types;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Filters;
using TG.Bot.TelegramApi.TestServise.Views;
using Telegram.Bot;

namespace TG.Bot.TelegramApi.TestServise.Handlers;

/// <summary>
/// Обработчик нажатия кнопки на стадии выбора теста
/// </summary>
/// <param name="_backendApi"></param>
internal class TestCallbackQueryHandler(IBackendApi _backendApi) : CallbackQueryHandler
{
    [StateFilter(nameof(States.SelectingTest))]
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

        await context.State.SetState(nameof(States.StartTest));
        await context.State.UpdateData(nameof(test), test!);
    }

    private async Task<GetTestDto?> GetTestAsync(TelegramContext context)
    {
        if (!Guid.TryParse(context.Data, out Guid testId))
            return null;

        var response = await _backendApi.GetTestAsync(testId);
        return response.Content;
    }

    private static bool IsValidTest(GetTestDto? test)
       => test != null && test.Questions != null && test.Questions.Count != 0;
}
