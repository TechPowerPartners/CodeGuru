using botovskiy.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System.Formats.Asn1;

internal class Program
{
    private static DiscordClient Client { get; set; }
    private static CommandsNextExtension Commands { get; set; }
    static async Task Main(string[] args)
    {
        
        var discordConfig = new DiscordConfiguration()
        {
            Intents = DiscordIntents.All,
            Token = "MTIyMDc0MTk3MTA5MDQ3NzA3Ng.GKY-xL.LQaznghYe049oS8Nm_qdoXGltB2FkFv4fYKoHM", //jsonReader.token
            TokenType = TokenType.Bot,
            AutoReconnect = true,
        };
        Client = new DiscordClient(discordConfig);

        Client.Ready += Client_Ready;

        Client.GuildMemberAdded += Client_GuildMemberAdded;

        var commandsConfig = new CommandsNextConfiguration()
        {
            StringPrefixes = new string[]
            {
                    "!"
            },
            EnableMentionPrefix = true,
            EnableDms = true,
            EnableDefaultHelp = false

        };
        Commands = Client.UseCommandsNext(commandsConfig);
        Commands.RegisterCommands<IsmeCommands>();

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    private static async Task Client_GuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e)
    {

        var guild = e.Guild;
        var generalChannel = guild.Channels.Values.FirstOrDefault(c => c.Name == "общее" && c.Type == ChannelType.Text);

        if (generalChannel == null)
        {
            Console.WriteLine("Channel not found");
            return;
        }


        var message = $"ДарооВа @{e.Member.Mention} в наш уютный {guild.Name}!\n Ты шарпист или же геймдейвер? Расскажи о себе(если не ответишь кик через 2 минуты)";
        await generalChannel.SendMessageAsync(message);
    }


    private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
    {
        return Task.CompletedTask;
    }
}
