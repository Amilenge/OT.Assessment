using Autofac;
using Autofac.Extensions.DependencyInjection;
using OT.Assessment.Core.DependencyInjection;
using OT.Assessment.Data.DependencyInjection;

namespace OT.Assessment.Api.DependencyInjection
{
    public class ApiConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            builder.RegisterInstance(configuration);

            builder.RegisterModule<RabbitMqModule>();
            builder.RegisterModule<CoreConfigurationModule>();

            builder.RegisterModule<DatabaseConfigurationModule>();

            builder.RegisterType<ApiWarmHostedService>()
                .As<IHostedService>()
                .SingleInstance();

            var services = new ServiceCollection();

            services.AddHttpClient();

            builder.Populate(services);
        }
    }
}
