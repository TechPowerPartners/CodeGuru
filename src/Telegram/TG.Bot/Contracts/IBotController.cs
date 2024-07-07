using Telegram.Bot.Types.Enums;
using TG.Bot.Common;

namespace TG.Bot.Contracts;

internal interface IBotController
{
    Task HandleUpdateAsync(TelegramContext context);
    List<UpdateType> UpdateTypes { get; }
}