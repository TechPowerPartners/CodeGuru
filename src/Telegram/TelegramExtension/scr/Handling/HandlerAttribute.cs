using Telegram.Bot.Types.Enums;

namespace TelegramBotExtension.Handling;

[AttributeUsage(AttributeTargets.Class)]
internal class HandlerAttribute(UpdateType updateType) : Attribute
{
    public UpdateType UpdateType { get; set; } = updateType;
}
