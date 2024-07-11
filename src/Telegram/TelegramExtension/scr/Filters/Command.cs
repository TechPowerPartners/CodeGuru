using Telegram.Bot.Types.Enums;
using TelegramBotExtension.Types;

namespace TelegramBotExtension.Filters;

public class Command(string command) : FilterAttribute(command)
{
    private bool IsValidCommand(TelegramContext baseContext)
    {
        var message = baseContext.Update.Message;

        if (message == null)
            return false;

        if (message.Entities == null || message.Entities.Length == 0)
            return false;

        if (message.Entities[0].Type != MessageEntityType.BotCommand)
            return false;

        return message.Text != null && message.Text == "/" + Data;
    }

    public override Task<bool> Call(TelegramContext context)
        => Task.FromResult(IsValidCommand(context));
}
