using System.Net.Http.Json;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Guard.Bot.Commands;

internal class EvalCommands(IHttpClientFactory clientFactory) : BaseCommandModule
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;

    public record EvalResult
    {
        public object? ReturnValue { get; set; }
        public string? ReturnTypeName { get; set; }
        public string? Exception { get; set; }
        public string? ExceptionType { get; set; }
        public string? Code { get; set; }
        public string? ConsoleOut { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public TimeSpan CompileTime { get; set; }
    }

    [Command("eval")]
    public async Task ProcessEvalCommand(CommandContext context, [RemainingText] string input)
    {
        var client = _clientFactory.CreateClient("eval");

        var content = new StringContent(input);

        try
        {
            var response = await client.PostAsync("http://localhost:31337/eval", content);
            
            var result = (await response.Content.ReadFromJsonAsync<EvalResult>())!;

            var messageBuilder = new DiscordEmbedBuilder()
                .WithTitle("Компилятор")
                .WithColor(DiscordColor.Gold);

            if (!string.IsNullOrEmpty(result.Code))
                messageBuilder.AddField("Код", $"```cs\n{result.Code}\n```");

            if (result.ReturnValue is not null)
                messageBuilder.AddField("Результат", $"```json\n{result.ReturnValue}\n```");

            if (!string.IsNullOrEmpty(result.ConsoleOut))
                messageBuilder.AddField("Вывод в консоль", $"```json\n{result.ConsoleOut}\n```");

            if (!string.IsNullOrEmpty(result.Exception))
                messageBuilder.AddField("Результат", $"```json\n{result.ExceptionType} - {result.Exception}\n```");

            messageBuilder.WithFooter(
                $"Время компиляции: {result.CompileTime}ms | Время работы: {result.ExecutionTime}ms");
            
            await context.RespondAsync(messageBuilder);
        }
        catch (Exception e)
        {
            await context.RespondAsync("Таймаут, спам или неправильный запрос");
            throw;
        }
    }
}