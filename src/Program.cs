using gifty.API.Bootstrapers;
using gifty.Shared.Builders;

namespace gifty.API
{
    public class Program
    {
        public static void Main(string[] args)
        =>
            ServiceBuilder
                .CreateDefault<Startup>()
                .WithPort(5000)
                .WithAutofac(AuthBootstraper.BootstraperLifetimeScope)
                .WithNoNeo4j()
                .WithRabbitMq("Users", "guest", "guest", 5672)
                .Build()
                .Run();
        
    }
}
