using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Guard.Api.Contracts.Users;
using Guard.Bot.Integrations.GuardApi;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace Guard.Bot.DiscordApi.EventSubscribers;

internal class ComponentInteractionCreatedEventSubscriber(IGuardApi _guardApi)
   : IDiscordComponentInteractionCreatedEventSubscriber
{
	public async Task DiscordOnComponentInteractionCreated(DiscordClient sender,
	   ComponentInteractionCreateEventArgs args)
	{
		if (args.Id == "panel-create-personal-acc")
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
		var exampleDate = $"{DateTime.Now.AddDays(1):dd.MM.yyyy}";
		var exampleTime = $"{DateTime.Now:HH:00}-{DateTime.Now.AddHours(2):HH:00}";

		var modal = new DiscordInteractionResponseBuilder()
		   .WithCustomId("id")
		   .WithTitle($"Запись на собеседование")
		   .AddComponents(new TextInputComponent("Дата", "date", exampleDate, exampleDate,
			  true, TextInputStyle.Short, 10, 10))
		   .AddComponents(new TextInputComponent("Время (МСК)", "time", exampleTime, exampleTime,
			  true, TextInputStyle.Short, 11, 11));

		await args.Interaction.CreateResponseAsync(InteractionResponseType.Modal, modal);
	}

	private async Task ProcessCreatePersonalAcc(ComponentInteractionCreateEventArgs args)
	{
		var member = await args.Guild.GetMemberAsync(args.User.Id);
		var discordDmChannel = await member.CreateDmChannelAsync();

		var password = GeneratePassword();

		var request = new RegisterRequest()
		{
			Name = member.Username,
			Password = password,
			Validator = 765123
		};

		var apiResponse = await _guardApi.RegisterAsync(request);

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