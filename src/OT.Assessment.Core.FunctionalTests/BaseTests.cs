using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OT.Assessment.Core.DependencyInjection;
using OT.Assessment.Data.Models;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using OT.Assessment.Core.Messaging;
using OT.Assessment.Core.Mapping;

namespace OT.Assessment.Core.FunctionalTests
{
    public abstract class BaseTests
    {
        protected IContainer _container;
        private DbConnection _connection;

        public BaseTests()
        {
            BuildContainer();
        }

        private void BuildContainer()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json")
                .Build();

            var builder = new ContainerBuilder();
            builder.RegisterInstance(configuration).As<IConfiguration>();
            builder.RegisterConsumers();
            builder.RegisterOtherConsumerDependencies(configuration);

            var services = new ServiceCollection();

            // Register DbContextFactory for SQLite in-memory
            services.AddDbContextFactory<OtAssessmentDbContext>(options =>
                options.UseSqlite(_connection)); // Use the same connection for the DbContextFactory

            RegisterInMemoryDb(builder);

            RegisterAutoMapper(builder);

            builder.Populate(services);

            _container = builder.Build();

            // Ensure the database schema is created after the container is built
            using var scope = _container.BeginLifetimeScope();
            var context = scope.Resolve<OtAssessmentDbContext>();
            context.Database.EnsureCreated();
        }

        private void RegisterInMemoryDb(ContainerBuilder builder)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open(); // Keep the connection open for the in-memory database

            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<OtAssessmentDbContext>()
                    .UseSqlite(_connection); // Use the shared in-memory connection

                return new OtAssessmentDbContext(optionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();
        }

        private void RegisterAutoMapper(ContainerBuilder builder)
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {
                // Load profiles from all assemblies
                cfg.AddMaps(typeof(CoreMapping).Assembly);
            });

            builder.RegisterInstance(new Mapper(mapConfig)).As<IMapper>();
        }

        protected ReadOnlyMemory<byte> GetReadOnlyMemory<T>(T message)
        {
            var byteArray = MessageSerializer.SerializeMessage(message);
            return new ReadOnlyMemory<byte>(byteArray);
        }

        public void Dispose()
        {
            // Ensure the connection is closed after the test runs
            _connection?.Close();
        }
    }
}