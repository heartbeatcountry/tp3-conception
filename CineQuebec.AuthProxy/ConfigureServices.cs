using CineQuebec.Application.Interfaces.Services;

using Microsoft.Extensions.DependencyInjection;

namespace CineQuebec.AuthProxy;

public static class ConfigureServices
{
    public static IServiceCollection WrapServicesWithAuthProxy(this IServiceCollection services)
    {
        return services
            .AjouterProxyAuthPourService<IActeurQueryService>();
    }
}