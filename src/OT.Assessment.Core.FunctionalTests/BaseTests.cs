using Autofac;
using OT.Assessment.Core.DependencyInjection;

namespace OT.Assessment.Core.FunctionalTests
{
    public class BaseTests
    {
        public BaseTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<RabbitMqModule>();
            builder.RegisterConsumers();
        }

        [Fact]
        public void Test1()
        {

        }
    }
}