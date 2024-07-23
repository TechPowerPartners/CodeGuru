using System.Diagnostics;
using Telegram.Bot;
using TelegramBotExtension.Filters;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Types;
using TG.Bot.Enums;
using TG.Bot.Intagrations.BackendApi;
using TG.Bot.Intagrations.TestingPlatformApi;
using TG.Bot.TelegramApi.TestService.Views;

namespace TG.Bot.TelegramApi.TestService.Handlers;

/// <summary>
/// Обработчик команды /tests. Отображаются список тестов в виде кнопок
/// </summary>
/// <param name="_testingPlatformApi"></param>
internal class TestsCommandHandler(
    ITestingPlatformApi _testingPlatformApi,
    IBackendApi _backendApi) : MessageHandler
{
    [Command("tests")]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        // нужна оптимизация (кэширование)
        var backendResponse = await _backendApi.GetUserIdByTelegramAsync(context.UserId);

        if (!backendResponse.IsSuccessStatusCode)
        {
            await context.BotClient.SendTextMessageAsync(
                context.UserId,
                "Авторизируйтесь с помощью команды /auth");
            return;
        }

        // TODO: Время выполнения запроса GetTestNamesAndIdsAsync в backend (276 мс)
        // нужна оптимизация (кэширование)
        var response = await _testingPlatformApi.GetTestNamesAndIdsAsync();

        if (!response.IsSuccessStatusCode) return;

        var testNamesAndIds = response.Content!;

        if (testNamesAndIds.Count == 0)
        {
            await context.BotClient.SendTextMessageAsync(
                context.UserId,
                "Тестов пока что нет, попробуйте позже... :(");
            return;
        }
        await TestsView.ShowAsync(context, testNamesAndIds);

        await context.State.Clear();
        await context.State.SetState(nameof(TestState.SelectingTest));
    }
}
