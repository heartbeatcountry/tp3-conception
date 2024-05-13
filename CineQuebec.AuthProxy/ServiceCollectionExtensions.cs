using System.Runtime.CompilerServices;
using System.Security;

using CineQuebec.Domain.Entities.Utilisateurs;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: InternalsVisibleTo("Tests.AuthProxy")]

namespace CineQuebec.AuthProxy;

public static class ServiceCollectionExtensions
{
    internal static readonly HashSet<string> AlreadyBoundServices = [];

    public static IServiceCollection AjouterProxyAuthPourService<TService>(this IServiceCollection services,
        Dictionary<Role, IEnumerable<string>> methodMapping)
        where TService : class
    {
        ValiderServiceNestPasUnDuplicat<TService>(methodMapping);
        ValiderAucuneMethodePubliqueNonMappee<TService>(methodMapping);

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

    private static void ValiderServiceNestPasUnDuplicat<TService>(Dictionary<Role, IEnumerable<string>> methodMapping)
        where TService : class
    {
        if (!AlreadyBoundServices.Add(typeof(TService).FullName ?? ""))
        {
            throw new InvalidOperationException(
                $"Le service {typeof(TService).Name} a déjà été ajouté au proxy d'authentification.");
        }

        string[] lstMethodes = methodMapping.SelectMany(m => m.Value).ToArray();

        if (lstMethodes.Length != lstMethodes.Distinct().Count())
        {
            throw new InvalidOperationException(
                "Il y a des méthodes dupliquées dans le mappage des rôles et des méthodes pour le service" +
                $" {typeof(TService).Name}. Veuillez réviser la configuration dans la couche AuthProxy.");
        }
    }

    private static void ValiderAucuneMethodePubliqueNonMappee<TService>(
        Dictionary<Role, IEnumerable<string>> methodMapping)
        where TService : class
    {
        IEnumerable<string> lstMethodes = typeof(TService).GetMethods()
            .Where(m => m is { IsPublic: true, IsSpecialName: false })
            .Select(m => m.Name);

        string[] methodesNonMappees = lstMethodes.Except(methodMapping.SelectMany(m => m.Value)).ToArray();

        if (methodesNonMappees.Length != 0)
        {
            throw new InvalidOperationException(
                $"Il y a des méthodes publiques non mapées pour le service {typeof(TService).Name}: " +
                $"{string.Join(", ", methodesNonMappees)}. Veuillez réviser la configuration dans la couche AuthProxy.");
        }
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