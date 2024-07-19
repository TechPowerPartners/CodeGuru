using Telegram.Bot;
using TelegramBotExtension.Filters;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Types;

namespace TG.Bot.TelegramApi.HelpService.Handlers;

internal class HelpCommandHandler : MessageHandler
{
    [Command("help")]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        await context.BotClient.SendTextMessageAsync(context.UserId, "Сам разберись!");
    }
}
