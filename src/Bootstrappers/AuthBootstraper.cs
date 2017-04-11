using Autofac;
using gifty.Api.Providers;
using gifty.Api.Auth;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using System.Linq;

namespace gifty.API.Bootstrapers
{
    internal sealed class AuthBootstraper : AutofacNancyBootstrapper
    {
        private readonly IContainer _owinContainer;
        private ILifetimeScope _lifetimeScope;
        public AuthBootstraper(IContainer owinContainer)
        {
            _owinContainer = owinContainer;
        }
        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            base.ConfigureApplicationContainer(_owinContainer);

            _owinContainer.Update(builder => 
            {
                builder.RegisterType<IdentityProvider>().As<IIdentityProvider>();      
            });
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            pipelines.BeforeRequest += (NancyContext ctx) => 
            {                
                return (ctx.CurrentUser == null && !AuthWhiteList.WhiteList.Any(p => p == ctx.Request.Path))? new Response() {StatusCode = HttpStatusCode.Unauthorized} : null;
            };
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {            
            var identityProvider = _owinContainer.Resolve<IIdentityProvider>();
            var statelessAuthConfig = new StatelessAuthenticationConfiguration(identityProvider.GetUserIdentity);

            StatelessAuthentication.Enable(pipelines, statelessAuthConfig);            
        }
    }
}