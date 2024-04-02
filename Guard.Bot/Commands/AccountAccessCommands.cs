using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Guard.Bot.GuardApiContracts.DTOs;

namespace Guard.Bot.Commands;

public class AccountAccessCommands(IGuardApi _guardApi) : BaseCommandModule
{
    [Command("dayparol")]
    public async Task SendPassword(CommandContext ctx)
    {
        var discordDmChannel = await ctx.Member.CreateDmChannelAsync();

        var password = GeneratePassword();

        var dto = new RegisterDto()
        {
            Name = ctx.Member.Username,
            Password = password,
            Validator = 765123
        };

        await _guardApi.RegisterAsync(dto);

        await discordDmChannel.SendMessageAsync($"Твой Логин: {ctx.Member.Username}\nТвой пароль: {password}");
    }

    private static string GeneratePassword()
        => Guid.NewGuid().ToString()[..8];
}