namespace gifty.API
{
    using Autofac;
    using gifty.Shared.Exceptions;
    using gifty.Shared.IoC;
    using gifty.Shared.ServiceBus;
    using Nancy;

    public class HomeModule : NancyModule
    {
        public HomeModule(IServiceBus serviceBus)
        {
            Get("/", args => throw new GiftyException(ErrorType.InvalidCommand, "Something failed"));
        }
    }
}
