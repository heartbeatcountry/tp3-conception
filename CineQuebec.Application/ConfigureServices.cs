using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services;

using Microsoft.Extensions.DependencyInjection;

namespace CineQuebec.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IFilmQueryService, FilmQueryService>()
            .AddSingleton<IFilmCreationService, FilmCreationService>();
    }
}