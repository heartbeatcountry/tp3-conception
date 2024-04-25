using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Security.Claims;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Utilisateurs;

using Microsoft.Extensions.DependencyInjection;

namespace CineQuebec.AuthProxy;

public class ServiceAuthProxy<TService> : DispatchProxy where TService : class
{
    private IAuthenticationService _authenticationService = null!;
    private Dictionary<string, Role> _methodMapping = [];
    private TService _targetService = null!;

    public static TService CreerDispatchProxy(TService service, IServiceProvider serviceProvider,
        IDictionary<Role, IEnumerable<string>> methodMapping)
    {
        ServiceAuthProxy<TService> proxy =
            (Create<TService, ServiceAuthProxy<TService>>() as ServiceAuthProxy<TService>)!;

        proxy._targetService = service;
        proxy._authenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();
        proxy._methodMapping = methodMapping
            .SelectMany(kvp => kvp.Value, (kvp, s) => new { Role = kvp.Key, Method = s })
            .ToDictionary(x => x.Method, x => x.Role);

        return (proxy as TService)!;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        Debug.Assert(targetMethod != null, nameof(targetMethod) + " != null");

        if (targetMethod.IsPublic)
        {
            if (!_methodMapping!.TryGetValue(targetMethod.Name, out Role roleRequis))
            {
                throw new SecurityException(
                    $"La méthode {typeof(TService).Name}.{targetMethod.Name} n'est pas présente dans le mapping de rôles.");
            }

            if (roleRequis != Role.Invite && !ClaimsPrincipalPossedeRole(roleRequis))
            {
                throw new SecurityException(
                    $"La méthode {typeof(TService).Name}.{targetMethod.Name} nécessite le rôle {roleRequis}.");
            }
        }

        return targetMethod.Invoke(_targetService, args);
    }

    private bool ClaimsPrincipalPossedeRole(Role roleRequis)
    {
        ClaimsPrincipal? claimsPrincipal = _authenticationService.ObtenirAutorisation();
        return claimsPrincipal?.IsInRole(roleRequis.ToString()) ?? false;
    }
}