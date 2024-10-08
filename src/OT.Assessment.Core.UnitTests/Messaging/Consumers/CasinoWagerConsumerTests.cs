using AutoMapper;
using Moq;
using OT.Assessment.Core.Mapping;
using OT.Assessment.Core.Messaging.Consumers;
using RabbitMQ.Client;

namespace OT.Assessment.Core.UnitTests.Messaging.Consumers
{
    public class CasinoWagerConsumerTests
    {
        private readonly Mock<IConnection> connectionMock;
        private readonly Mock<IModel> channelMock;
        private readonly CasinoWagerConsumer sut;

        public CasinoWagerConsumerTests()
        {
            connectionMock = new Mock<IConnection>();

            channelMock = new Mock<IModel>();
            connectionMock.Setup(x => x.CreateModel()).Returns(channelMock.Object);
            channelMock.Setup(x => x.QueueDeclare(It.IsAny<string>(), true, default, default, default))
                .Returns(new QueueDeclareOk("", 1, 1));
            channelMock.Setup(x => x.CreateBasicProperties())
                .Returns(Mock.Of<IBasicProperties>());

            var mapConfig = new MapperConfiguration(cfg =>
            {
                // Load profiles from all assemblies
                cfg.AddMaps(typeof(CoreMapping).Assembly);
            });
        }
    }
}
