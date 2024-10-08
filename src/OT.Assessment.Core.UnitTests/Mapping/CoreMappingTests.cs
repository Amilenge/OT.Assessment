using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using OT.Assessment.Api.Contract.Request;
using OT.Assessment.Consumer.Mapping;
using OT.Assessment.Core.Dtos;
using OT.Assessment.Data.Models;

namespace OT.Assessment.Core.UnitTests.Mapping
{
    public class CoreMappingTests
    {
        private IMapper sut;

        private MapperConfiguration config;
        private Fixture _fixture;

        public CoreMappingTests()
        {
            config = new MapperConfiguration(cfg =>
            {
                // Load profiles from all assemblies
                cfg.AddMaps(typeof(CoreMapping).Assembly);
            });

            sut = config.CreateMapper();

            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        }

        [Fact]
        public void Map_From_CasinoWagerRequest_To_Wager()
        {
            // Arrange
            var src = _fixture.Create<Wager>(); // avoid circular dependency issue

            // Act
            var dest = sut.Map<WagerDto>(src);

            dest.Should().NotBeNull();
            dest.WagerId.Should().Be(src.WagerId);
            // .. other field assertion
        }

        [Fact]
        public void Map_From_CasinoWagerRequest_To_Tranaction()
        {
            // Arrange
            var src = _fixture.Create<Account>();

            // Act
            var dest = sut.Map<AccountDto>(src);

            dest.Should().NotBeNull();
            dest.AccountId.Should().Be(src.AccountId);
            dest.Username.Should().Be(src.Username);
            // .. other field assertion

        }
    }
}
