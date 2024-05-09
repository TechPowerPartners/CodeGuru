using DC.Bot.Integrations.Queue.Consumers;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DC.Bot.Integrations.Queue;

internal static class Entry
{
	public static IServiceCollection ConfigureQueue(this IServiceCollection services, IConfiguration configuration)
	{
		var bus = RabbitHutch.CreateBus("host=rabbitmq;username=rabbitmq;password=rabbitmq", register =>
		{
			register.EnableSystemTextJson();
			register.EnableConsoleLogger();
		});

		services.AddSingleton(bus);

		services.AddScoped<InterviewCreatedConsumer>();

		services.AddHostedService<QueueSubscriberHostedService>();

		return services;
	}
}
