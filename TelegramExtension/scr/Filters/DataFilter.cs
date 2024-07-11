using TelegramBotExtension.Types;

namespace TelegramBotExtension.Filters;

public class DataFilter(string? data) : FilterAttribute(data)
{
    public override Task<bool> Call(TelegramContext context)
        => Task.FromResult(Data == context.Data);
}
