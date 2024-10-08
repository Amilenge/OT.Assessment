using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using OT.Assessment.Api.Contract.Request;
using OT.Assessment.Consumer.Mapping;
using OT.Assessment.Data.Models;

namespace OT.Assessment.Consumer.UnitTests.Mapping
{
    public class ApplicationMappingTests
    {
        private IMapper sut;

        private MapperConfiguration config;

        public ApplicationMappingTests()
        {
            config = new MapperConfiguration(cfg =>
            {
                // Load profiles from all assemblies
                cfg.AddMaps(typeof(ConsumerMapping).Assembly);
            });

            sut = config.CreateMapper();
        }

        [Theory]
        [AutoData]
        public void Map_From_CasinoWagerRequest_To_Wager(CasinoWagerRequest src)
        {
            // Act
            var dest = sut.Map<Wager>(src);

            dest.Should().NotBeNull();
            dest.WagerId.Should().Be(src.WagerId);
            dest.AccountId.Should().Be(src.AccountId);
            // .. other field assertion
        }

        [Theory]
        [AutoData]
        public void Map_From_CasinoWagerRequest_To_Tranaction(CasinoWagerRequest src)
        {
            // Act
            var dest = sut.Map<Transaction>(src);

            dest.Should().NotBeNull();
            dest.WagerId.Should().Be(src.WagerId);
            dest.TransactionId.Should().Be(src.TransactionId);
            // .. other field assertion

        }

        [Theory]
        [AutoData]
        public void Map_From_CasinoWagerRequest_To_Account(CasinoWagerRequest src)
        {
            // Act
            var dest = sut.Map<Account>(src);

            dest.Should().NotBeNull();
            dest.Username.Should().Be(src.Username);
            dest.AccountId.Should().Be(src.AccountId);
            dest.TotalAmountSpend.Should().Be(0);
            // .. other field assertion

        }
    }
}
