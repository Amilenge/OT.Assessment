using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using OT.Assessment.Consumer.Mapping;
using OT.Assessment.Core.DependencyInjection;

namespace OT.Assessment.Consumer.DependencyInjection
{
    public class ConsumerConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            builder.Register(ctx =>
            {
                var mapConfig = new MapperConfiguration(cfg =>
                {
                    // Load profiles from all assemblies
                    cfg.AddMaps(typeof(ConsumerMapping).Assembly);
                });

                return mapConfig.CreateMapper();
            }).As<IMapper>().SingleInstance();

            builder.RegisterOtherConsumerDependencies(configuration);

            //builder.RegisterInstance(configuration);

            //builder.RegisterModule<RabbitMqModule>();

            //builder.RegisterModule<DatabaseConfigurationModule>();

            //builder.Register(ctx =>
            //{
            //    var mapConfig = new MapperConfiguration(cfg =>
            //    {
            //        // Load profiles from all assemblies
            //        cfg.AddMaps(typeof(ConsumerMapping).Assembly);
            //    });

            //    return mapConfig.CreateMapper();
            //}).As<IMapper>().SingleInstance();

            //builder.RegisterConsumers();

            //var services = new ServiceCollection();

            //services.AddDbContextFactory<OtAssessmentDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection")));

            //builder.Populate(services);
        }
    }
}
