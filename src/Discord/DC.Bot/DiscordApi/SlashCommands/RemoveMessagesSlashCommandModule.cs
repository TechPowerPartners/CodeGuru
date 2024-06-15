using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System.Globalization;

namespace DC.Bot.DiscordApi.SlashCommands;

/// <summary>
/// Модуль slash команд относящихся к фичи удаления сообщений из чата по фильтрам
/// </summary>
public class RemoveMessagesSlashCommandModule : ApplicationCommandModule
{
	private const string Prefix = "remove-messages-";
    private const int NumberOfDeletedMessages = 15;

    /// <summary>
    /// Удаляет сообщения которые были отправлены после указанной даты и времени
    /// </summary>
    [SlashCommand(Prefix + "after", "Удалить все сообщения после указанного времени")]
	public async Task RemoveMessagesAfter(
		InteractionContext ctx,
		[Option("date", "Дата (ДД.ММ.ГГГГ)")] string dateString,
        [Option("time", "Время (ЧЧ:ММ:СС)")] TimeSpan? time)
	{
        if (!CheckAccess(ctx))
            return;

        var isValidDate = DateTime.TryParseExact(dateString, "dd.MM.yyyy", provider: null, DateTimeStyles.None, out var date);

		if (!isValidDate || !time.HasValue)
            return;

		var afterDateTime = date.Add(time.Value);
		var messages = ctx.Channel.GetMessagesAsync(limit: NumberOfDeletedMessages);

        List<DiscordMessage> messagesToDelete = [];
        await foreach(var message in messages)
			if(message.Timestamp > afterDateTime)
                messagesToDelete.Add(message);

        if(messagesToDelete.Count != 0)
            await ctx.Channel.DeleteMessagesAsync(messagesToDelete);

        await Complete(ctx);
    }

    /// <summary>
    /// Удаляет сообщения которые были отправлены определенным пользователем
    /// </summary>
    [SlashCommand(Prefix + "user", "Удалить все сообщения пользователя")]
    public async Task RemoveMessagesByUser(
        InteractionContext ctx,
        [Option("user", "Пользователь, чьи сообщения будут удалены")] DiscordUser user)
    {
        if (!CheckAccess(ctx))
            return;

        var messages = ctx.Channel.GetMessagesAsync(limit: NumberOfDeletedMessages);

        List<DiscordMessage> messagesToDelete = [];

        await foreach (var message in messages)
            if (message.Author?.Id == user.Id)
                messagesToDelete.Add(message);

        if (messagesToDelete.Count != 0)
            await ctx.Channel.DeleteMessagesAsync(messagesToDelete);

        await Complete(ctx);
    }

    private static bool CheckAccess(InteractionContext ctx)
    {
        const string TeamLeadRole = "🧙‍♂️ Team Lead";
        return ctx.Member.Roles.Any(dr => dr.Name == TeamLeadRole);
    }

    private static async Task Complete(InteractionContext ctx)
    {
        await ctx.CreateResponseAsync("Дело сделано");
        await ctx.DeleteResponseAsync();
    }
}
