using DSharpPlus;
using DSharpPlus.EventArgs;
using Guard.Bot.Integrations.GuardApi;
using Guard.Bot.Integrations.GuardApi.DTOs;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace Guard.Bot.SubscriberModules;

internal class DiscordComponentInteractionCreatedEventSubscriber(IGuardApi _guardApi) : IDiscordComponentInteractionCreatedEventSubscriber
{
    public async Task DiscordOnComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs args)
    {
        if(args.Id == "panel-create-personal-acc")
        {
            await ProcessCreatePersonalAcc(args);
        }

        if (args.Id == "panel-shedule-interview")
        {
            await ProcessSheduleInterview(args);
        }
    }

    private async Task ProcessSheduleInterview(ComponentInteractionCreateEventArgs args)
    {
    }

    private async Task ProcessCreatePersonalAcc(ComponentInteractionCreateEventArgs args)
    {
        var member = await args.Guild.GetMemberAsync(args.User.Id);
        var discordDmChannel = await member.CreateDmChannelAsync();

        var password = GeneratePassword();

        var dto = new RegisterDto()
        {
            Name = member.Username,
            Password = password,
            Validator = 765123
        };

        var apiResponse = await _guardApi.RegisterAsync(dto);

        if (apiResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            await discordDmChannel.SendMessageAsync("Ты уже зареган, вспоминай пароль анчоус");
            return;
        }

        if (!apiResponse.IsSuccessStatusCode)
        {
            await discordDmChannel.SendMessageAsync("Опять нихрена не работает. Тех неполадки емае");
            return;
        }

        await discordDmChannel.SendMessageAsync($"Твой Логин: {member.Username}\nТвой пароль: {password}");
    }

    private static string GeneratePassword()
        => Guid.NewGuid().ToString()[..8];
}
