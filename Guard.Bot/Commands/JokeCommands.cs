using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Guard.Bot.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Guard.Bot.Commands;

internal class JokeCommands(
    IGuardApi _guardApi,
    IOptions<ResourceSettings> _resourceSettingsOptions) : BaseCommandModule
{
    [Command("joke")]
    public async Task ProcessJokeCommand(CommandContext context)
    {
        var message = await CreateJokeMessage();

        await context.Channel.SendMessageAsync(message);
    }

    private async Task<string> CreateJokeMessage()
    {
        var random = new Random();
        var source = random.Next(0, 2);

        var isSuccessApiResponse = false;

        if (source == 0)
        {
            var apiResponse = await _guardApi.GetRandomPostContentAsync();
            isSuccessApiResponse = apiResponse.IsSuccessStatusCode;

            if (apiResponse.IsSuccessStatusCode)
                return apiResponse.Content;
        }

        if (!isSuccessApiResponse || source > 0)
        {
            var json = await File.ReadAllTextAsync(_resourceSettingsOptions.Value.IsmaJokesPath);
            var jokes = JsonConvert.DeserializeObject<List<dynamic>>(json);

            dynamic randomJoke = jokes[random.Next(jokes.Count)];

            return "```!Запомните ребята, безделье - игрушка дьявола,и так к анекдоту:```\n" + randomJoke.joke.ToString();
        }

        return "Технические неполадки :(";
    }
}
