using System.Reflection;
using System.Text;
using System.Text.Json;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Guard.Bot.Commands;

internal class EvalCommands : BaseCommandModule
{
    [Command("eval")]
    public async Task ProcessEvalCommand(CommandContext context, params string[] input)
    {
        var options = ScriptOptions.Default.AddImports("System", "System.IO", "System.Collections.Generic",
                "System.Console", "System.Diagnostics", "System.Dynamic",
                "System.Linq", "System.Text",
                "System.Threading.Tasks", "Newtonsoft.Json")
            .AddReferences("System", "System.Core", "Microsoft.CSharp", "System.Text.dll", "Newtonsoft.Json");

        var code = string.Join(' ', input);

        try
        {
            var script = CSharpScript.Create(code, options);
            script.Compile();
            var state = await script.RunAsync();
            var result = state.Variables.Select(v => v.Name + " = " + JsonSerializer.Serialize(v.Value));

            var output = new StringBuilder();
            foreach (var line in result)
            {
                output.Append(line);
                output.Append("\n\n");
            }

            var beautyCode = string.Join("\n",
                code.Split(";").Where(s => s != string.Empty).Select(s => s.Trim() + ";"));

            var builder = new DiscordEmbedBuilder()
                .WithTitle("Компилятор")
                .WithColor(DiscordColor.Gold)
                .AddField("Код", $"```cs\n{beautyCode}\n```")
                .AddField("Вывод", $"```json\n{output}\n```");

            await context.RespondAsync(builder);
        }
        catch (Exception e)
        {
            await context.RespondAsync(e.Message);
        }
    }
}