namespace gifty.API
{
    using Autofac;
    using gifty.Shared.Exceptions;
    using gifty.Shared.IoC;
    using gifty.Shared.ServiceBus;
    using Nancy;
    using Nancy.Security;

    public class HomeModule : NancyModule
    {
        public HomeModule(IServiceBus serviceBus)
        {
            this.RequiresAuthentication();
            Get("/", args => "Hello world!");
        }
    }
}
