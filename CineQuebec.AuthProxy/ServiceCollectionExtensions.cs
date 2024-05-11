using System.Security;

using CineQuebec.Domain.Entities.Utilisateurs;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CineQuebec.AuthProxy;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AjouterProxyAuthPourService<TService>(this IServiceCollection services,
        Dictionary<Role, IEnumerable<string>> methodMapping)
        where TService : class
    {
        foreach (ServiceDescriptor descriptor in services
                     .Where(s => s.ServiceType == typeof(TService)).ToArray())
        {
            services.Replace(CreerNouveauServiceDescriptor<TService>(descriptor, methodMapping));
        }

        return services;
    }

    public static IServiceCollection BloquerToutAutreServiceSansAutorisationExplicite(this IServiceCollection services)
    {
        foreach (ServiceDescriptor descriptor in services.Where(d =>
                     !ConfigureServices.ServicesAutorisesSansProxy.Contains(d.ServiceType) &&
                     (d.ServiceType?.Name?.EndsWith("Service") ?? false) &&
                     d.ImplementationFactory is null))
        {
            throw new SecurityException(
                $"Le service {descriptor.ServiceType.Name} n'est pas autorisé à être utilisé sans proxy d'authentification. " +
                "Le service doit être ajouté à ConfigureServices dans la couche AuthProxy.");
        }

        return services;
    }

    private static ServiceDescriptor CreerNouveauServiceDescriptor<TService>(ServiceDescriptor ancienDescriptor,
        IDictionary<Role, IEnumerable<string>> methodMapping)
        where TService : class
    {
        return ServiceDescriptor
            .Describe(
                typeof(TService),
                serviceProvider => ServiceAuthProxy<TService>.CreerDispatchProxy(
                    serviceProvider.CreerInstanceDeService<TService>(ancienDescriptor), serviceProvider, methodMapping),
                ancienDescriptor.Lifetime
            );
    }

    private static TService CreerInstanceDeService<TService>(this IServiceProvider services,
        ServiceDescriptor descriptor) where TService : class
    {
        return (TService)(descriptor.ImplementationInstance ?? (descriptor.ImplementationFactory != null
            ? descriptor.ImplementationFactory(services)
            : ActivatorUtilities.GetServiceOrCreateInstance(services,
                descriptor.ImplementationType ??
                throw new InvalidOperationException("Impossible de trouver l'implémentation du service"))));
    }
}