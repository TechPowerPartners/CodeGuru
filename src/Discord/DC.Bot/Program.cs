using DC.Bot.Common.Settings;
using DC.Bot.DiscordApi;
using DC.Bot.Integrations;
using DSharpPlus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nefarius.DSharpPlus.Extensions.Hosting;
using System.Reflection;

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

		services.AddDiscordHostedService();

		services.ConfigureDiscordApi(hostContext.Configuration);
		services.ConfigureIntergrations(hostContext.Configuration);
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
