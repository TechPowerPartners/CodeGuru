using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Guard.Bot.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Guard.Bot.Commands;

internal class AnecdoteCommands(
    IGuardApi _guardApi,
    IOptions<ResourceSettings> _resourceSettingsOptions) : BaseCommandModule
{
    [Command("daygazu")]
    public async Task GenericIsma(CommandContext context)
    {
        var json = await File.ReadAllTextAsync(_resourceSettingsOptions.Value.IsmaJokesPath);
        var jokes = JsonConvert.DeserializeObject<List<dynamic>>(json);

        var random = new Random();
        dynamic randomJoke = jokes[random.Next(jokes.Count)];

        await context.Channel.SendMessageAsync("```!Запомните ребята, безделье - игрушка дьявола,и так к анекдоту:```\n" + randomJoke.joke.ToString());
    }

    [Command("sila")]
    public async Task GenericCitation(CommandContext context)
    {
        var apiResponse = await _guardApi.GetRandomPostContentAsync();

        var message = apiResponse.IsSuccessStatusCode ? 
            apiResponse.Content : 
            "Опять нихрена не работает. Тех неполадки";

        await context.Channel.SendMessageAsync(message);
    }
}
