using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Net;
using System.Net.Http.Json;

namespace Guard.Bot.DiscordApi.PrefixCommands;

internal class EvalCommands(IHttpClientFactory clientFactory) : BaseCommandModule
{
	private readonly IHttpClientFactory _clientFactory = clientFactory;

	[Command("eval")]
	public async Task ProcessEvalCommand(CommandContext context, [RemainingText] string input)
	{
		await context.TriggerTypingAsync();

		var uri = Environment.GetEnvironmentVariable("EvalApiUrl");

		if (uri is null)
			throw new Exception("EvilApiUrl environment variable not provided");

		uri += "/eval";

		var client = _clientFactory.CreateClient();

		HttpResponseMessage r;
		try
		{
			r = await client.PostAsync(uri, JsonContent.Create(new { Code = input }));
		}
		catch (Exception)
		{
			await context.RespondAsync("Таймаут, попробуй еще раз");
			return;
		}

		if (r.StatusCode != HttpStatusCode.BadRequest && r.StatusCode != HttpStatusCode.OK)
		{
			await context.RespondAsync("Сервак сдох");
			return;
		}

		var response = (await r.Content.ReadFromJsonAsync<Response>())!;

		if (r.StatusCode == HttpStatusCode.BadRequest)
		{
			await context.RespondAsync(response.Error!);
		}

		var result = response.Result!;

		var builder = new DiscordEmbedBuilder()
			.WithTitle("Компилятор")
			.WithColor(DiscordColor.Gold);

		int remain = 1000;

		if (!string.IsNullOrEmpty(result.Code))
		{
			builder.WithDescription($"Код\n```cs\n{result.Code}\n```");
		}

		if (result.ReturnValue is not null)
		{
			var json = response.SerializedReturnValue!;

			remain -= Truncate(ref json, 800);

			builder.AddField("Результат", $"```json\n{json}\n```");
		}

		if (!string.IsNullOrEmpty(result.ConsoleOut))
		{
			var json = result.ConsoleOut;

			Truncate(ref json, remain);

			builder.AddField("Вывод в консоль",
				$"```json\n{json}\n```");
		}

		if (!string.IsNullOrEmpty(result.Exception))
			builder.AddField("Результат", $"```diff\n{result.ExceptionType} - {result.Exception}\n```");

		builder.WithFooter(
			$"Время компиляции: {result.CompileTime}ms | Время работы: {result.ExecutionTime}ms");

		await context.RespondAsync(builder.Build());
		await context.Channel.DeleteMessageAsync(context.Message);
	}

	private static int Truncate(ref string str, int limit)
	{
		var len = str.Length;

		if (len <= limit) return len;

		str = string.Concat(str.AsSpan(0, limit), "...");
		return limit;
	}
}

public class Response
{
	public int Status { get; set; }
	public string? Error { get; set; }
	public string? SerializedReturnValue { get; set; }
	public EvalResult? Result { get; set; }
}

public class EvalResult
{
	public object? ReturnValue { get; set; }
	public string? Exception { get; set; }
	public string? ExceptionType { get; set; }
	public string? Code { get; set; }
	public string? ConsoleOut { get; set; }
	public TimeSpan ExecutionTime { get; set; }
	public TimeSpan CompileTime { get; set; }
}