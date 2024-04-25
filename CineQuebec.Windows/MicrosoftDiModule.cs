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
        IServiceCollection serviceCollection = ConfigureServices();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        foreach (ServiceDescriptor service in serviceCollection)
        {
            RegisterServiceWithStyletIoC(service, serviceProvider);
        }
    }

    private static IServiceCollection ConfigureServices()
    {
        return new ServiceCollection()
            .AddPersistenceServices(GetConfiguration())
            .AddApplicationServices()
            .WrapServicesWithAuthProxy();
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
        IAsWeakBinding serviceReg = Bind(service.ServiceType)
            .ToInstance(serviceProvider.GetRequiredService(service.ServiceType));

        if (service.Lifetime == ServiceLifetime.Singleton && serviceReg is IInScopeOrAsWeakBinding singleton)
        {
            singleton.InSingletonScope();
        }
    }
}