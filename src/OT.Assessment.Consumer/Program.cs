using Autofac;
using Autofac.Extensions.DependencyInjection;
using OT.Assessment.Consumer.DependencyInjection;
using OT.Assessment.Core.Messaging.Consumers;
using RabbitMQ.Client;

var host = Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory(builder =>
    {
        builder.RegisterModule<ConsumerConfigurationModule>();
    }))
    .ConfigureServices((context, services) =>
    {
        services.AddLogging(configure => configure.AddConsole());
    })
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started {time:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

var consumers = host.Services.GetServices<IConsumer>();

foreach(var consumer in consumers)
{
    var channel = host.Services.GetService<IModel>();

    channel.BasicConsume(
        queue: consumer.QueueName,
        autoAck: true,
        consumer: consumer
    );
}

await host.RunAsync();

logger.LogInformation("Application ended {time:yyyy-MM-dd HH:mm:ss}", DateTime.Now);