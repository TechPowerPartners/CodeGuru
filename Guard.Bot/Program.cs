using System.Reflection;
using DSharpPlus;
using Guard.Bot.Commands;
using Guard.Bot.Integrations;
using Guard.Bot.Queue;
using Guard.Bot.Settings;
using Guard.Bot.SubscriberModules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting;
using Nefarius.DSharpPlus.Extensions.Hosting;
using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<ResourceSettings>(hostContext.Configuration.GetSection(nameof(ResourceSettings)));
        services.Configure<DiscordServerSettings>(hostContext.Configuration.GetSection(nameof(DiscordServerSettings)));
        services.AddDiscord(config =>
        {
            config.Intents = DiscordIntents.All;
            config.Token = hostContext.Configuration.GetValue<string>("BotSettings:Token")!;
            config.TokenType = TokenType.Bot;
            config.AutoReconnect = true;
        });

        services.AddDiscordGuildMemberAddedEventSubscriber<GuildMemberEventsSubscriberModule>();
        services.AddDiscordComponentInteractionCreatedEventSubscriber<DiscordComponentInteractionCreatedEventSubscriber>();
        services.AddDiscordModalSubmittedEventSubscriber<DiscordModalSubmittedEventSubscriber>();
        
        services.AddDiscordCommandsNext(
            options =>
            {
                options.StringPrefixes = [hostContext.Configuration.GetValue<string>("BotSettings:CommandPrefix")!];
                options.EnableDms = false;
                options.EnableMentionPrefix = true;
                options.EnableDefaultHelp = false;
            },
            extension =>
            {
                extension.RegisterCommands<CoreCommands>();
                extension.RegisterCommands<JokeCommands>();
                extension.RegisterCommands<EvalCommands>();
                
            });

        services.AddDiscordSlashCommands(extension: extension =>
        {
            
        });

        services.AddDiscordHostedService();

        services.ConfigureIntergrations(hostContext.Configuration);
        services.ConfigureQueue(hostContext.Configuration);
    });

builder.ConfigureAppConfiguration(conf =>
{
    conf.AddJsonFile("appsettings.json", optional: false, true)
        .AddJsonFile("appsettings.Secrets.json", optional: true, true)
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
        .AddEnvironmentVariables();
});

var app = builder.Build();

await app.RunAsync();
