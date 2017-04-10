using Autofac;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;

namespace gifty.API.Bootstrapers
{
    public class AuthBootstraper : AutofacNancyBootstrapper
    {
        private readonly IContainer _owinContainer;
        public AuthBootstraper(IContainer owinContainer)
        {
            _owinContainer = owinContainer;
        }
        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            base.ConfigureApplicationContainer(container);

            container.Update(builder => 
            {
                
                foreach(var registration in _owinContainer.ComponentRegistry.Registrations)
                    builder.RegisterComponent(registration);
            });
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            var config = new StatelessAuthenticationConfiguration(nancyContext =>
                    {
                        //for now, we will pull the apiKey from the querystring,
                        //but you can pull it from any part of the NancyContext
                        var apiKey = (string) nancyContext.Request.Query.ApiKey.Value;

                        //get the user identity however you choose to (for now, using a static class/method)
                        return new System.Security.Claims.ClaimsPrincipal();
                    });

            StatelessAuthentication.Enable(pipelines, config);

            pipelines.BeforeRequest += (NancyContext ctx) => {
                return new Response { StatusCode = HttpStatusCode.Unauthorized };
            };
        }
    }
}