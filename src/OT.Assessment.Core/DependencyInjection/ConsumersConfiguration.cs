using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OT.Assessment.Core.Messaging.Consumers;
using OT.Assessment.Data.DependencyInjection;
using OT.Assessment.Data.Models;
using System.Reflection;

namespace OT.Assessment.Core.DependencyInjection
{
    public static class ConsumersConfiguration
    {
        public static Dictionary<string, Type> RegisteredConsumerTypes = new Dictionary<string, Type>();

        public static ContainerBuilder RegisterConsumers(this ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => typeof(IConsumer).IsAssignableFrom(t) && !t.IsAbstract)
                .As<IConsumer>()
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder RegisterOtherConsumerDependencies(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterInstance(configuration);

            builder.RegisterModule<RabbitMqModule>();

            builder.RegisterModule<DatabaseConfigurationModule>();

            builder.RegisterConsumers();

            var services = new ServiceCollection();

            services.AddDbContextFactory<OtAssessmentDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection")));

            builder.Populate(services);

            return builder;
        }
    }
}
