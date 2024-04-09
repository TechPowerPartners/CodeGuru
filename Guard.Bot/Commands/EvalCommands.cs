using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AngouriMath;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Hosting;
using TypeInfo = System.Reflection.TypeInfo;

namespace Guard.Bot.Commands;

internal class EvalCommands(IHttpClientFactory clientFactory) : BaseCommandModule
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;

    [Command("eval")]
    public async Task ProcessEvalCommand(CommandContext context, [RemainingText] string input)
    {
        var uri = Environment.GetEnvironmentVariable("EvilApiUrl");
        
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
            await context.RespondAsync("Таймаут, ты заепал циклы запускать");
            return;
        }
        
        if (r.StatusCode != HttpStatusCode.BadRequest && r.StatusCode != HttpStatusCode.OK)
        {
            await context.RespondAsync("Сервак сдох");
            return;
        }

        var response = (await r.Content.ReadFromJsonAsync<Response>())!;
        
        if(r.StatusCode == HttpStatusCode.BadRequest)
        {
            await context.RespondAsync(response.Error!);
        }
        
        var result = response.Result;

        var builder = new DiscordEmbedBuilder()
            .WithTitle("Компилятор")
            .WithColor(DiscordColor.Gold);

        if (!string.IsNullOrEmpty(result.Code))
        {
            builder.WithDescription($"Код\n```cs\n{result.Code}\n```");
        }

        if (result.ReturnValue is not null)
        {
            builder.AddField("Результат", $"```json\n{(response.SerializedReturnValue!.Length > 600
                ? response.SerializedReturnValue[..600] + "..."
                : response.SerializedReturnValue)}\n```");
        }

        if (!string.IsNullOrEmpty(result.ConsoleOut))
        {
            builder.AddField("Вывод в консоль",
                $"```json\n{(result.ConsoleOut.Length > 600 ? result.ConsoleOut[..600] + "..." : result.ConsoleOut)}\n```");
        }

        if (!string.IsNullOrEmpty(result.Exception))
            builder.AddField("Результат", $"```diff\n{result.ExceptionType} - {result.Exception}\n```");

        builder.WithFooter(
            $"Время компиляции: {result.CompileTime}ms | Время работы: {result.ExecutionTime}ms");

        await context.RespondAsync(builder.Build());
        await context.Channel.DeleteMessageAsync(context.Message);
    }
}

public class Response
{
    public int Status { get; set; }
    public string? Error { get; set; }
    public string? SerializedReturnValue { get; set; }
    public EvalResult Result { get; set; }
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