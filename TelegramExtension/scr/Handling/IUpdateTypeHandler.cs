using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotExtension.Types;

namespace TelegramBotExtension.Handling;

public interface IUpdateTypeHandler
{
    Task HandleUpdateAsync(TelegramContext context);
    TelegramContext GetContext(ITelegramBotClient botClient, Update update);
}