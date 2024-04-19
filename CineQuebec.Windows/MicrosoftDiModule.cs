using System.IO;

using CineQuebec.Application;
using CineQuebec.Persistence;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StyletIoC;

namespace CineQuebec.Windows;

internal class MicrosoftDiModule : StyletIoCModule
{
    protected override void Load()
    {
        IConfiguration configuration = GetConfiguration();
        IServiceCollection serviceCollection = ConfigureServices(configuration);

        foreach (ServiceDescriptor service in serviceCollection)
        {
            IAsWeakBinding serviceReg = service switch
            {
                { ImplementationInstance: not null } => Bind(service.ServiceType)
                    .ToInstance(service.ImplementationInstance),
                { ImplementationType: not null } => Bind(service.ServiceType).To(service.ImplementationType),
                _ => throw new InvalidOperationException("Unsupported service implementation")
            };

            if (service.Lifetime == ServiceLifetime.Singleton && serviceReg is IInScopeOrAsWeakBinding singleton)
            {
                singleton.InSingletonScope();
            }
        }
    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile("appsettings.local.json", true, true)
            .Build();
    }

    private static IServiceCollection ConfigureServices(IConfiguration configuration)
    {
        return new ServiceCollection()
            .AddPersistenceServices(configuration)
            .AddApplicationServices();
    }
}