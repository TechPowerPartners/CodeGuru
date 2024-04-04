using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Guard.Bot.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Guard.Bot.Commands;

internal class SlashCommands(IOptions<ResourceSettings> _resourceSettingsOptions) : ApplicationCommandModule
{
    [SlashCommand("poshytit", "Шуткануть над")]
    public async Task Joke(
        InteractionContext context, 
        [Option("пользователь", "Пользователь для шутки")] DiscordUser user)
    {
        var json = await File.ReadAllTextAsync(_resourceSettingsOptions.Value.IsmaJokesPath);
        var jokes = JsonConvert.DeserializeObject<List<dynamic>>(json);

        var random = new Random();
        dynamic randomJoke = jokes[random.Next(jokes.Count)].joke;
        string joke = randomJoke.ToString();

        var member = await context.Guild.GetMemberAsync(user.Id);

        var replacedJoke = joke.Replace("Исмаил", member.DisplayName);

        await context.Channel.SendMessageAsync(replacedJoke);
    }

    [SlashCommand("hrm", "Меню HRM")]
    public async Task ShowMenu(InteractionContext context)
    {
        List<DiscordComponent> firstRow = [
            new DiscordButtonComponent(ButtonStyle.Success, "create-acc", "Создать учетную запись"),
            new DiscordButtonComponent(ButtonStyle.Secondary, "reset-acc", "Сбросить пароль для учетной записи")
            ];

        List<DiscordComponent> secondRow = [
            new DiscordButtonComponent(ButtonStyle.Primary, "fix", "Зафиксировать время", true),
            new DiscordLinkButtonComponent("https://techpowerpartners.github.io/", "Ресурс", true),
        ];

        var builder = new DiscordMessageBuilder()
            .WithContent("Меню Human Management System")
            .AddComponents(firstRow)
            .AddComponents(secondRow);

        await context.Channel.SendMessageAsync(builder);
    }
}
