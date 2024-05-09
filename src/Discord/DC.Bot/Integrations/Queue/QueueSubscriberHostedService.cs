using DC.Bot.Integrations.Queue.Consumers;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Queue.Contracts;

namespace DC.Bot.Integrations.Queue;
internal class QueueSubscriberHostedService(IServiceScopeFactory _serviceScopeFactory) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		const string subscriberId = "defualt_id";

		var scope = _serviceScopeFactory.CreateScope();
		var bus = scope.ServiceProvider.GetRequiredService<IBus>();

		await bus.PubSub.SubscribeAsync<InterviewCreatedMessage>(subscriberId, async message =>
		{
			var consumer = scope.ServiceProvider.GetRequiredService<InterviewCreatedConsumer>();
			await consumer.ConsumeAsync(message);
		}, cancellationToken: stoppingToken);
	}
}
