using Autofac;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OT.Assessment.Api.Contract.Request;
using OT.Assessment.Core.Messaging.Consumers;
using OT.Assessment.Data.Models;

namespace OT.Assessment.Core.FunctionalTests
{
    public class CasinoWagerConsumerTests : BaseTests
    {
        private readonly CasinoWagerConsumer sut;

        public CasinoWagerConsumerTests()
            : base()
        {
            sut = _container.Resolve<CasinoWagerConsumer>();
        }

        [Theory]
        [AutoData]
        public async Task CasinoWagerRequest_Cosumed_Then_Save_To_DB(CasinoWagerRequest request)
        {
            // Arrange
            var body = GetReadOnlyMemory(request);

            // Act
            sut.HandleBasicDeliver("consumerTag", 0, false, "exchange", "routerkey", null, body);

            // Assert
            var wager = await _container.Resolve<OtAssessmentDbContext>()
                .Wagers
                .FirstOrDefaultAsync(x => x.WagerId == request.WagerId);
            wager.Should().NotBeNull();
        }

        // ... more tests here
    }
}
