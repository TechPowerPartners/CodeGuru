using TelegramBotExtension.Filters;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Types;
using TG.Bot.enums;
using TG.Bot.Intagrations.BackendApi;

namespace TG.Bot.TelegramApi.AuthService.Handlers;

internal class EnterPasswordMessageHandler(IBackendApi backendApi) : MessageHandler
{
    [StateFilter(nameof(AuthState.Password))]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        var userData = await context.State.GetData();
        var name = (string)userData[nameof(AuthState.Name)];
        var password = context.Data;

        var response = await backendApi.LoginAsync(new() {Name = name, Password = password });

        await context.State.Clear();
    }
}
