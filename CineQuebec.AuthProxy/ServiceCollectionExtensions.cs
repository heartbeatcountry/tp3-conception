using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CineQuebec.AuthProxy;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AjouterProxyAuthPourService<TService>(this IServiceCollection services)
        where TService : class
    {
        MethodInfo factoryMethod = typeof(ServiceAuthProxy<TService>)
                                       .GetMethods(BindingFlags.Public | BindingFlags.Static)
                                       .FirstOrDefault(info =>
                                           !info.IsGenericMethod && info.ReturnType == typeof(TService))
                                   ?? throw new InvalidOperationException("Impossible de trouver le factory method");

        ParameterInfo[] factoryMethodArgs = factoryMethod.GetParameters();

        foreach (ServiceDescriptor descriptor in services
                     .Where(s => s.ServiceType == typeof(TService)).ToArray())
        {
            ServiceDescriptor decorated = ServiceDescriptor
                .Describe(
                    typeof(TService),
                    sp =>
                    {
                        object? decoratorInstance = factoryMethod.Invoke(null,
                            factoryMethodArgs.Select(
                                    info => info.ParameterType ==
                                            (descriptor.ServiceType ?? descriptor.ImplementationType)
                                        ? sp.CreateInstance(descriptor)
                                        : sp.GetRequiredService(info.ParameterType))
                                .ToArray());
                        return decoratorInstance as TService ??
                               throw new InvalidOperationException("Impossible de créer le proxy de service");
                    },
                    descriptor.Lifetime
                );

            services.Replace(decorated);
        }

        return services;
    }

    private static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
    {
        return descriptor.ImplementationInstance ?? (descriptor.ImplementationFactory != null
            ? descriptor.ImplementationFactory(services)
            : ActivatorUtilities.GetServiceOrCreateInstance(services,
                descriptor.ImplementationType ??
                throw new InvalidOperationException("Impossible de trouver l'implémentation")));
    }
}