using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services;
using CineQuebec.Application.Services.Abstract;

using Microsoft.Extensions.DependencyInjection;

namespace CineQuebec.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IPasswordHashingService, PasswordHashingService>()
            .AddSingleton<IAuthenticationService, AuthenticationService>()
            .AddSingleton<IFilmQueryService, FilmQueryService>()
            .AddSingleton<IFilmCreationService, FilmCreationService>()
            .AddSingleton<IFilmDeletionService, FilmDeletionService>()
            .AddSingleton<IFilmUpdateService, FilmUpdateService>()
            .AddSingleton<IActeurCreationService, ActeurCreationService>()
            .AddSingleton<IActeurQueryService, ActeurQueryService>()
            .AddSingleton<IRealisateurCreationService, RealisateurCreationService>()
            .AddSingleton<IRealisateurQueryService, RealisateurQueryService>()
            .AddSingleton<ICategorieFilmCreationService, CategorieFilmCreationService>()
            .AddSingleton<ICategorieFilmQueryService, CategorieFilmQueryService>()
            .AddSingleton<IProjectionCreationService, ProjectionCreationService>()
            .AddSingleton<IProjectionQueryService, ProjectionQueryService>()
            .AddSingleton<IProjectionDeletionService, ProjectionDeletionService>()
            .AddSingleton<ISalleCreationService, SalleCreationService>()
            .AddSingleton<ISalleQueryService, SalleQueryService>();
    }
}