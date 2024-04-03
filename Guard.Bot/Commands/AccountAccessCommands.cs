using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Guard.Bot.GuardApiContracts.DTOs;

namespace Guard.Bot.Commands;

public class AccountAccessCommands(IGuardApi _guardApi) : BaseCommandModule
{
    [Command("dayparol")]
    public async Task SendPassword(CommandContext context)
    {
        var discordDmChannel = await context.Member.CreateDmChannelAsync();

        var password = GeneratePassword();

        var dto = new RegisterDto()
        {
            Name = context.Member.Username,
            Password = password,
            Validator = 765123
        };

        var apiResponse = await _guardApi.RegisterAsync(dto);

        if(apiResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            await context.Channel.SendMessageAsync("Ты уже зареган, вспоминай пароль анчоус");
            return;
        }

        if (!apiResponse.IsSuccessStatusCode)
        {
            await context.Channel.SendMessageAsync("Опять нихрена не работает. Тех неполадки емае");
            return;
        }

        await discordDmChannel.SendMessageAsync($"Твой Логин: {context.Member.Username}\nТвой пароль: {password}");
    }

    private static string GeneratePassword()
        => Guid.NewGuid().ToString()[..8];
}