using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using Moq;
using OT.Assessment.Api.Contract.Request;
using OT.Assessment.Core.Messaging.Senders;
using RabbitMQ.Client;

namespace OT.Assessment.Core.UnitTests.Messaging.Senders
{
    public class SendersTests
    {
        private readonly Mock<IConnection> connectionMock;
        private readonly Mock<IModel> channelMock;
        private readonly CasinoWagerRequestSender sut;

        public SendersTests()
        {
            connectionMock = new Mock<IConnection>();
            sut = new CasinoWagerRequestSender(
                connectionMock.Object, 
                Mock.Of<ILogger<CasinoWagerRequestSender>>());

            channelMock = new Mock<IModel>();
            connectionMock.Setup(x => x.CreateModel()).Returns(channelMock.Object);
            channelMock.Setup(x => x.QueueDeclare(It.IsAny<string>(), true, default, default, default))
                .Returns(new QueueDeclareOk("", 1, 1));
            channelMock.Setup(x => x.CreateBasicProperties())
                .Returns(Mock.Of<IBasicProperties>());
        }

        [Theory]
        [AutoData]
        public async Task CasinoWagerRequestSender_When_SendAsync_CasinoWagerRequest(CasinoWagerRequest request)
        {
            // Arrange

            // Act
            await sut.SendAsync(request);

            // Assert
            channelMock.Verify(x => x.BasicPublish(
                "",
                It.IsAny<string>(),
                true,
                It.IsAny<IBasicProperties>(),
                It.IsAny<ReadOnlyMemory<byte>>()));
        }

        // ...more tests
    }
}
