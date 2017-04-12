using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using gifty.Api.Settings;
using gifty.API.Bootstrapers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;

namespace gifty.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }
        public IContainer _selfContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));
           
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            _selfContainer = containerBuilder.Build();

            return _selfContainer.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin().UseNancy(x => x.Bootstrapper = new AuthBootstraper(_selfContainer, Configuration));
        }
    }
}
