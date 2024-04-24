using System.IO;

using CineQuebec.Application;
using CineQuebec.AuthProxy;
using CineQuebec.Persistence;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StyletIoC;

namespace CineQuebec.Windows;

internal class MicrosoftDiModule : StyletIoCModule
{
    protected override void Load()
    {
        (IServiceCollection serviceCollection, IServiceProvider serviceProvider) = ConfigureServices();

        foreach (ServiceDescriptor service in serviceCollection)
        {
            RegisterServiceWithStyletIoC(service, serviceProvider);
        }
    }

    private static (IServiceCollection serviceCollection, IServiceProvider serviceProvider) ConfigureServices()
    {
        IServiceCollection serviceCollection = new ServiceCollection()
            .AddPersistenceServices(GetConfiguration())
            .AddApplicationServices()
            .WrapServicesWithAuthProxy();

        return (serviceCollection, serviceCollection.BuildServiceProvider());
    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile("appsettings.local.json", true, true)
            .Build();
    }

    private void RegisterServiceWithStyletIoC(ServiceDescriptor service, IServiceProvider serviceProvider)
    {
        IAsWeakBinding serviceReg = service switch
        {
            { ImplementationInstance: not null } => Bind(service.ServiceType)
                .ToInstance(service.ImplementationInstance),
            { ImplementationType: not null } => Bind(service.ServiceType).To(service.ImplementationType),
            { ImplementationFactory: not null } => Bind(service.ServiceType)
                .ToInstance(serviceProvider.GetRequiredService(service.ServiceType)),
            _ => throw new InvalidOperationException("Unsupported service implementation")
        };

        if (service.Lifetime == ServiceLifetime.Singleton && serviceReg is IInScopeOrAsWeakBinding singleton)
        {
            singleton.InSingletonScope();
        }
    }
}