using DSharpPlus;
using Guard.Bot.Commands;
using Guard.Bot.SubscriberModules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting;
using Nefarius.DSharpPlus.Extensions.Hosting;
using Refit;

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddDiscord(config =>
        {
            config.Intents = DiscordIntents.All;
            config.Token = "MTIyMDc0MTk3MTA5MDQ3NzA3Ng.GKY-xL.LQaznghYe049oS8Nm_qdoXGltB2FkFv4fYKoHM";
            config.TokenType = TokenType.Bot;
            config.AutoReconnect = true;
        });

        services.AddDiscordGuildMemberAddedEventSubscriber<GuildMemberEventsSubscriberModule>();

        services.AddDiscordCommandsNext(
            options =>
            {
                options.StringPrefixes = ["!"];
                options.EnableDms = false;
                options.EnableMentionPrefix = true;
            },
            extension =>
            {
                extension.RegisterCommands<CoreCommands>();
                extension.RegisterCommands<AnecdoteCommands>();
                extension.RegisterCommands<AccountAccessCommands>();
            });

        services.AddDiscordHostedService();

        services
            .AddRefitClient<IGuardApi>()
            .ConfigureHttpClient(client => client.BaseAddress = new ("http://127.0.0.1:5184"));
    });

var app = builder.Build();

await app.StartAsync();
