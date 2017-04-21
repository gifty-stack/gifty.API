using Autofac;
using gifty.Api.Providers;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Microsoft.Extensions.Configuration;
using gifty.Api.Settings;
using gifty.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using gifty.Shared.Nancy;

namespace gifty.API.Bootstrapers
{
    internal sealed class AuthBootstraper : BootstrapperBase
    {
        private readonly IConfigurationRoot _configurationRoot;

        public AuthBootstraper(IServiceCollection services, IConfigurationRoot configurationRoot)
                :base(services)                
        {
            _configurationRoot = configurationRoot;
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        { 
            container.Update(builder => 
            {
                builder.RegisterType<IdentityProvider>().As<IIdentityProvider>();
                builder.RegisterInstance(_configurationRoot.RegisterSetting<AuthSettings>(nameof(AuthSettings))).SingleInstance();
            });

            base.ConfigureApplicationContainer(container);
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            var identityProvider = BootstraperLifetimeScope.Resolve<IIdentityProvider>();
            var statelessAuthConfig = new StatelessAuthenticationConfiguration(identityProvider.GetUserIdentity);

            StatelessAuthentication.Enable(pipelines, statelessAuthConfig);   

            base.ApplicationStartup(container, pipelines);
        }
    }
}