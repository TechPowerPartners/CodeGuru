using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;

namespace Guard.Bot.Commands;

public class AnecdoteCommands(IGuardApi _guardApi) : BaseCommandModule
{
    [Command("daygazu")]
    public async Task GenericIsma(CommandContext context)
    {
        var json = await File.ReadAllTextAsync("isme.json");
        var jokes = JsonConvert.DeserializeObject<List<dynamic>>(json);

        var random = new Random();
        dynamic randomJoke = jokes[random.Next(jokes.Count)];

        await context.Channel.SendMessageAsync("```!Запомните ребята, безделье - игрушка дьявола,и так к анекдоту:```\n" + randomJoke.joke.ToString());
    }

    [Command("sila")]
    public async Task GenericCitation(CommandContext context)
    {
        var postContent = await _guardApi.GetRandomPostContentAsync();
        await context.Channel.SendMessageAsync(postContent);
    }
}
