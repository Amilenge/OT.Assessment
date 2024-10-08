using Autofac;
using AutoMapper;
using OT.Assessment.Consumer.Mapping;

namespace OT.Assessment.Core.DependencyInjection
{
    public class CoreConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PlayerService>()
                .As<IPlayerService>()
                .SingleInstance();

            builder.Register(ctx =>
            {
                var mapConfig = new MapperConfiguration(cfg => cfg.AddMaps(typeof(CoreMapping).Assembly));

                return mapConfig.CreateMapper();
            }).As<IMapper>().SingleInstance();
        }
    }
}
