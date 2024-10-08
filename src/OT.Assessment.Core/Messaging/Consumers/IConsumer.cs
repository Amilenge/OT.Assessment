using RabbitMQ.Client;

namespace OT.Assessment.Core.Messaging.Consumers
{
    public interface IConsumer : IBasicConsumer
    {
        string QueueName { get; }
    }
}
