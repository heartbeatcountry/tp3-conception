using CineQuebec.Domain.Entities.Utilisateurs;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CineQuebec.AuthProxy;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AjouterProxyAuthPourService<TService>(this IServiceCollection services,
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