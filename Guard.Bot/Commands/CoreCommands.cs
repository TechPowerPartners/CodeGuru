using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Guard.Bot.Commands;

public class CoreCommands : BaseCommandModule
{
    [Command("pomogi")]
    public async Task GetCommands(CommandContext ctx)
    {
        await ctx.Channel.SendMessageAsync("""
            Команды который существуют:
            !dayparol - для получение своего логина и пароля
            !sila - для получение рандомной АУФ цитатки добавленные пользователями
            !daygazu - для получение анекдота(рандомно)
            """);
    }
}
