using Telegram.Bot;
using TelegramBotExtension.Filters;
using TelegramBotExtension.Handling;
using TelegramBotExtension.Types;

namespace TG.Bot.TelegramApi.StartService;

internal class StartCommandHandler : MessageHandler
{
    [Command("start")]
    public override async Task HandleUpdateAsync(TelegramContext context)
    {
        await context.BotClient.SendTextMessageAsync(
            context.UserId,
            "Приветствую тебя в Code Guru, чтобы пользоваться ботом, " +
            "пожалуйста, пройди регистрацию на нашем сайте(ссылка) или " +
            "дискорд-канале (ссылка) с помощью команды /auth"
            );
    }
}
