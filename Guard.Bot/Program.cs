using DSharpPlus;
using Guard.Bot.Commands;
using Guard.Bot.Settings;
using Guard.Bot.SubscriberModules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting;
using Nefarius.DSharpPlus.Extensions.Hosting;
using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting;
using Refit;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<ResourceSettings>(hostContext.Configuration.GetSection(nameof(ResourceSettings)));

        services.AddDiscord(config =>
        {
            config.Intents = DiscordIntents.All;
            config.Token = hostContext.Configuration.GetValue<string>("BotSettings:Token")!;
            config.TokenType = TokenType.Bot;
            config.AutoReconnect = true;
        });

        services.AddDiscordGuildMemberAddedEventSubscriber<GuildMemberEventsSubscriberModule>();

        services.AddDiscordCommandsNext(
            options =>
            {
                options.StringPrefixes = [hostContext.Configuration.GetValue<string>("BotSettings:CommandPrefix")!];
                options.EnableDms = false;
                options.EnableMentionPrefix = true;
            },
            extension =>
            {
                //extension.RegisterCommands<CoreCommands>();
                extension.RegisterCommands<AnecdoteCommands>();
                
            });

        services.AddDiscordSlashCommands(extension: extension =>
        {
            extension.RegisterCommands<SlashCommands>();
        });

        services.AddDiscordHostedService();

        services
            .AddRefitClient<IGuardApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new(hostContext.Configuration.GetValue<string>("ApiUrl")!);
            });
    });

builder.ConfigureAppConfiguration(conf =>
{
    conf.AddJsonFile("appsettings.json", optional: false, true)
        .AddJsonFile("appsettings.Secrets.json", optional: true, true)
        .AddEnvironmentVariables();
});

var app = builder.Build();

await app.RunAsync();
