using System.Reflection;

namespace CineQuebec.AuthProxy;

internal class ServiceAuthProxy<TService> : DispatchProxy where TService : class
{
    private TService _targetService = null!;

    public static TService CreateProxy(TService service)
    {
        if (Create<TService, ServiceAuthProxy<TService>>() is not ServiceAuthProxy<TService> proxy)
        {
            throw new InvalidOperationException("Impossible de créer le proxy");
        }

        proxy._targetService = service;

        return (proxy as TService)!;
    }

    protected override object Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        return targetMethod?.Invoke(_targetService, args)!;
    }
}