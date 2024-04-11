using Domain.Shared;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Guard.Api.Contracts.Interviews;
using Guard.Bot.Integrations.GuardApi;
using Guard.Bot.Settings;
using Microsoft.Extensions.Options;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace Guard.Bot.SubscriberModules;

internal class DiscordModalSubmittedEventSubscriber(
   IGuardApi _guardApi,
   IOptions<DiscordServerSettings> _tppServerSettings) : IDiscordModalSubmittedEventSubscriber
{
	public async Task DiscordOnModalSubmitted(DiscordClient sender, ModalSubmitEventArgs args)
	{
		var guild = await sender.GetGuildAsync(_tppServerSettings.Value.Id);
		var member = await guild.GetMemberAsync(args.Interaction.User.Id);

		var isValid = TryParseCreateInterviewRequest(member, args.Values, out var interviewDto);

		if (!isValid)
		{
			var errorEmbed = new DiscordEmbedBuilder()
			   .WithTitle("Введены неверные данные")
			   .AddField("Вводите данные в следующем формате", "Дата: dd.mm.yyyy \n Время: hh:mm-hh:mm")
			   .WithColor(DiscordColor.Red);

			await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
			   new DiscordInteractionResponseBuilder().AddEmbed(errorEmbed));
			return;
		}

		var response = await _guardApi.CreateInterviewAsync(interviewDto);

		if (!response.IsSuccessStatusCode)
		{
			var errorEmbed = new DiscordEmbedBuilder()
			   .WithTitle("Bad request")
			   .AddField("Error", response.Error.Content)
			   .WithColor(DiscordColor.Red);

			await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
			   new DiscordInteractionResponseBuilder().AddEmbed(errorEmbed));
			return;
		}

		var discordEmbedBuilder = new DiscordEmbedBuilder()
		   .WithTitle("Запрос на собеседование отправлен")
		   .AddField("Текущая роль", $"{interviewDto.FromRole.GetRealName()}")
		   .AddField("Следующая роль", $"{interviewDto.ToRole.GetRealName()}")
		   .AddField("С", $"{interviewDto.FromDate:dd.MM.yyyy-hh:mm}")
		   .AddField("По", $"{interviewDto.ToDate:dd.MM.yyyy-hh:mm}")
		   .WithColor(DiscordColor.Green);

		await args.Interaction.CreateResponseAsync(
		   InteractionResponseType.ChannelMessageWithSource,
		   new DiscordInteractionResponseBuilder().AddEmbed(discordEmbedBuilder));
	}

	private bool TryParseCreateInterviewRequest(
	   DiscordMember member,
	   IReadOnlyDictionary<string, string> values,
	   out CreateInterviewRequest request)
	{
		request = null!;
		var inputDate = values["date"];
		var inputTimeParts = values["time"].Split("-");

		if (inputTimeParts.Length != 2)
			return false;

		if (!DateOnly.TryParse(inputDate, out var date) ||
			!TimeOnly.TryParse(inputTimeParts[0], out var from) ||
			!TimeOnly.TryParse(inputTimeParts[1], out var to))
			return false;

		if (to <= from)
			return false;

		var currentRole = member.Roles.FirstOrDefault(r => CareerRoleExtensions.IsRealRoleValid(r.Name));
		var role = CareerRoleExtensions.GetFromRealName(currentRole.Name);

		request = new CreateInterviewRequest
		{
			FromDate = date.ToDateTime(from),
			ToDate = date.ToDateTime(to),
			FromRole = role,
			ToRole = role.BoostByInterview(),
			IntervieweeName = member.Username
		};

		return true;
	}
}