using DC.Bot.Common;
using DC.Bot.Common.Settings;
using DSharpPlus;
using DSharpPlus.Entities;
using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.Options;
using Nefarius.DSharpPlus.Extensions.Hosting;
using Queue.Contracts;

namespace DC.Bot.Integrations.Queue.Consumers;

internal class InterviewCreatedConsumer(
	IDiscordClientService _discordClientService,
	IOptions<DiscordServerSettings> _tppServerSettings) : IConsumeAsync<InterviewCreatedMessage>
{
	public async Task ConsumeAsync(InterviewCreatedMessage message, CancellationToken cancellationToken = default)
	{
		var guild = await _discordClientService.Client.GetGuildAsync(_tppServerSettings.Value.Id);

		var interviewModerationChannel = guild.Channels.Values.FirstOrDefault(c => c.Name == ChannelNameConsts.InterviewModeration);

		var members = await guild.SearchMembersAsync(message.Name);

		var discordEmbedBuilder = new DiscordEmbedBuilder()
			.AddField("Участник", members.First().DisplayName)
			.AddField("Текущая роль", $"{message.FromRole}")
			.AddField("Следующая роль", $"{message.ToRole}")
			.AddField("Дата проведения", $"{message.FromTime:dd.MM.yyyy}. С {message.FromTime}. По {message.ToTime}")
			.WithTitle("Создано новое собеседование")
			.WithColor(DiscordColor.Orange);

		List<DiscordComponent> firstRow = [
			new DiscordButtonComponent(ButtonStyle.Success, "interview-created-accept", "Подтвердить"),
			new DiscordButtonComponent(ButtonStyle.Danger, "interview-created-decline", "Отказать"),
			];

		var messageBuilder = new DiscordMessageBuilder()
			.AddComponents(firstRow)
			.AddEmbed(discordEmbedBuilder);

		await interviewModerationChannel!.SendMessageAsync(messageBuilder);
	}
}
