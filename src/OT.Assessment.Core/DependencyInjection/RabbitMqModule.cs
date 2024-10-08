using Autofac;
using OT.Assessment.Core.Messaging;
using Microsoft.Extensions.Configuration;
using OT.Assessment.Core.Messaging.Senders;
using RabbitMQ.Client;
using OT.Assessment.Api.Contract.Request;

namespace OT.Assessment.Core.DependencyInjection
{
    public class RabbitMqModule : Module
    {
        private static readonly object _connectionLock = new object();

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx =>
            {
                var configuration = ctx.Resolve<IConfiguration>();

                 return configuration
                    .GetSection("RabbitMqSettings")
                    .Get<RabbitMqSettings>();
            }).As<IRabbitMqSettings>().SingleInstance();

            builder.RegisterType<CasinoWagerRequestSender>()
                .As<IMessageSender<CasinoWagerRequest>>()
                .AsSelf()
                .SingleInstance();

            builder.Register(ctx =>
            {
                var _rabbitMqConfiguration = ctx.Resolve<IRabbitMqSettings>();

                var factory = new ConnectionFactory
                {
                    HostName = _rabbitMqConfiguration.HostName,
                    Port = _rabbitMqConfiguration.Port,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                };

                return factory.CreateConnection();

            }).As<IConnection>().SingleInstance();

            builder.Register(ctx =>
            {
                var rabbitMqConnection = ctx.Resolve<IConnection>();

                var channel = rabbitMqConnection.CreateModel();

                channel.QueueDeclare(
                    queue: Constants.CasinoWagerQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                return channel;
            }).As<IModel>().InstancePerDependency();
        }
    }
}
