using TelegramBotExtension.Types;

namespace TelegramBotExtension.Filters;

public class StateFilter(string state) : FilterAttribute(state)
{
    public override async Task<bool> Call(TelegramContext context)
        => await context.State.GetState() == Data;
}
