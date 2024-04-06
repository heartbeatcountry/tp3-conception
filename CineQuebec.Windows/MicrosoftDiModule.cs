using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineQuebec.Application;
using CineQuebec.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StyletIoC;

namespace CineQuebec.Windows
{
    internal class MicrosoftDiModule: StyletIoCModule
    {
        private static IConfiguration GetConfiguration ()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .Build();
        }

        private static IServiceCollection ConfigureServices (IConfiguration configuration)
        {
            return new ServiceCollection()
                .AddPersistenceServices(configuration)
                .AddApplicationServices();
        }

        protected override void Load()
        {
            var configuration = GetConfiguration();
            var serviceCollection = ConfigureServices(configuration);

            foreach (var service in serviceCollection)
            {
                IAsWeakBinding serviceReg = service switch
                {
                    { ImplementationInstance: not null } => Bind(service.ServiceType).ToInstance(service.ImplementationInstance),
                    { ImplementationType: not null } => Bind(service.ServiceType).To(service.ImplementationType),
                    _ => throw new InvalidOperationException("Unsupported service implementation"),
                };

                if (service.Lifetime == ServiceLifetime.Singleton && serviceReg is IInScopeOrAsWeakBinding singleton)
                {
                    singleton.InSingletonScope();
                }
            }
        }
    }
}
