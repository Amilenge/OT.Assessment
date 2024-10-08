using OT.Assessment.Api.Contract.Request;
using RabbitMQ.Client;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace OT.Assessment.Core.Messaging.Senders
{
    public class CasinoWagerRequestSender : IMessageSender<CasinoWagerRequest>, IDisposable
    {
        private readonly IConnection _connection;
        private readonly ConcurrentQueue<IModel> _channelPool;
        private readonly ILogger<CasinoWagerRequestSender> _logger;
        private readonly SemaphoreSlim _poolSemaphore;

        private const int MaxPoolSize = 50;

        public CasinoWagerRequestSender(
            IConnection connection, 
            ILogger<CasinoWagerRequestSender> logger)
        {
            _connection = connection;
            _logger = logger;
            _channelPool = new ConcurrentQueue<IModel>();
            _poolSemaphore = new SemaphoreSlim(MaxPoolSize);

            // Pre-fill the channel pool
            for (int i = 0; i < MaxPoolSize; i++)
            {
                _channelPool.Enqueue(CreateChannel());
            }
        }

        private IModel CreateChannel()
        {
            var channel = _connection.CreateModel();

            channel.QueueDeclare(
                queue: Constants.CasinoWagerQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            return channel;
        }

        public async Task SendAsync(CasinoWagerRequest message)
        {
            await _poolSemaphore.WaitAsync();

            try
            {
                if (!_channelPool.TryDequeue(out var channel))
                {
                    channel = CreateChannel();
                }

                // Serialize message to JSON
                var body = MessageSerializer.SerializeMessage(message);

                // Set basic properties for the message
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(
                    exchange: "",
                    routingKey: Constants.CasinoWagerQueueName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);

                // Log the success
                _logger.LogInformation("Message published successfully to {0}", Constants.CasinoWagerQueueName);

                // Re-enqueue the channel for reuse
                _channelPool.Enqueue(channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message to queue");
                throw;
            }
            finally
            {
                // Release the semaphore
                _poolSemaphore.Release();
            }
        }

        public void Dispose()
        {
            // Dispose channels when sender is disposed
            while (_channelPool.TryDequeue(out var channel))
            {
                channel?.Dispose();
            }
        }
    }
}