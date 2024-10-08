using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OT.Assessment.Data.Models;

namespace OT.Assessment.Data.DependencyInjection
{
    public class DatabaseConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var configuration = c.Resolve<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DatabaseConnection");

                var optionsBuilder = new DbContextOptionsBuilder<OtAssessmentDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                return new OtAssessmentDbContext(optionsBuilder.Options);
            }).AsSelf().InstancePerLifetimeScope();
        }
    }
}
