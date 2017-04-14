using Autofac;
using gifty.Api.Providers;
using gifty.Api.Auth;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using System.Linq;
using Microsoft.Extensions.Configuration;
using gifty.Api.Settings;
using gifty.Shared.Extensions;
using gifty.Shared.IoC;

namespace gifty.API.Bootstrapers
{
    internal sealed class AuthBootstraper : AutofacNancyBootstrapper
    {
        internal static ILifetimeScope BootstraperLifetimeScope;
        private readonly IContainer _owinContainer;
        private readonly IConfigurationRoot _configurationRoot;

        public AuthBootstraper(IContainer owinContainer, IConfigurationRoot configurationRoot)
        {
            _owinContainer = owinContainer;
            _configurationRoot = configurationRoot;
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            base.ConfigureApplicationContainer(container);

            container.Update(builder => 
            {                
                builder.RegisterModule(new ServiceModule());
                builder.RegisterType<IdentityProvider>().As<IIdentityProvider>();
                builder.RegisterInstance(_configurationRoot.RegisterSetting<AuthSettings>(nameof(AuthSettings))).SingleInstance();     

                foreach(var registry in _owinContainer.ComponentRegistry.Registrations)
                    builder.RegisterComponent(registry);
            });

            BootstraperLifetimeScope = container;
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
            var identityProvider = BootstraperLifetimeScope.Resolve<IIdentityProvider>();
            var statelessAuthConfig = new StatelessAuthenticationConfiguration(identityProvider.GetUserIdentity);

            StatelessAuthentication.Enable(pipelines, statelessAuthConfig);            
        }
    }
}