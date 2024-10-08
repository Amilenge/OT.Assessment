using AutoFixture.Xunit2;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using OT.Assessment.Api.Contract.Request;
using OT.Assessment.Api.Controllers;
using OT.Assessment.Core.Mapping;
using OT.Assessment.Core;
using OT.Assessment.Core.Messaging.Senders;
using OT.Assessment.Data.Models;
using RabbitMQ.Client;

namespace OT.Assessment.Api.UnitTests.Controllers
{
    public class PlayerControllerTests
    {
        private readonly PlayerController sut;

        private readonly Mock<IConnection> connectionMock;
        private readonly Mock<IModel> channelMock;

        public PlayerControllerTests()
        {
            connectionMock = new Mock<IConnection>();

            var requestSender = new CasinoWagerRequestSender(
                connectionMock.Object, 
                Mock.Of<ILogger<CasinoWagerRequestSender>>());

            channelMock = new Mock<IModel>();
            connectionMock.Setup(x => x.CreateModel()).Returns(channelMock.Object) ;
            channelMock.Setup(x => x.QueueDeclare(It.IsAny<string>(), true, default, default, default))
                .Returns(new QueueDeclareOk("", 1, 1));
            channelMock.Setup(x => x.CreateBasicProperties())
                .Returns(Mock.Of<IBasicProperties>());

            var mapConfig = new MapperConfiguration(cfg =>
            {
                // Load profiles from all assemblies
                cfg.AddMaps(typeof(CoreMapping).Assembly);
            });

            sut = new PlayerController(
                new PlayerService(
                    requestSender,
                    Mock.Of<OtAssessmentDbContext>(),
                    Mock.Of<IMapper>()));
        }

        [Theory, AutoData]
        public async Task When_PostCasinoWager(CasinoWagerRequest casinoWager)
        {
            // Arrange


            // Act
            await sut.PostCasinoWager(casinoWager);

            // Assert
            channelMock.Verify(x=> x.QueueDeclare(It.IsAny<string>(), true, default, default, default));
            channelMock.Verify(x=> x.BasicPublish(
                "",
                It.IsAny<string>(),
                true,
                It.IsAny<IBasicProperties>(),
                It.IsAny<ReadOnlyMemory<byte>>()));
        }

        // ... more tests
    }
}