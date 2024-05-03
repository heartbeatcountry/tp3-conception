using CineQuebec.Application.Interfaces.Services.Fidelite;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Services.Fidelite;
using CineQuebec.Application.Services.Films;
using CineQuebec.Application.Services.Identity;
using CineQuebec.Application.Services.Preferences;
using CineQuebec.Application.Services.Projections;

using Microsoft.Extensions.DependencyInjection;

namespace CineQuebec.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IPasswordValidationService, PasswordValidationService>()
            .AddSingleton<IPasswordHashingService, PasswordHashingService>()
            .AddSingleton<IUtilisateurAuthenticationService, UtilisateurAuthenticationService>()
            .AddSingleton<IUtilisateurCreationService, UtilisateurCreationService>()
            .AddSingleton<IUtilisateurFideliteQueryService, UtilisateurFideliteQueryService>()
            .AddSingleton<IFilmQueryService, FilmQueryService>()
            .AddSingleton<IBilletQueryService, BilletQueryService>()
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
            .AddSingleton<IBilletCreationService, BilletCreationService>()
            .AddSingleton<ISalleCreationService, SalleCreationService>()
            .AddSingleton<INoteFilmCreationService, NoteFilmCreationService>()
            .AddSingleton<ISalleQueryService, SalleQueryService>()
            .AddSingleton<ICategoriesPrefereesQueryService, CategoriesPrefereesQueryService>()
            .AddSingleton<ICategoriesPrefereesUpdateService, CategoriesPrefereesUpdateService>()
            .AddSingleton<IActeursFavorisQueryService, ActeursFavorisQueryService>()
            .AddSingleton<IActeursFavorisUpdateService, ActeursFavorisUpdateService>()
            .AddSingleton<IRealisateursFavorisQueryService, RealisateursFavorisQueryService>()
            .AddSingleton<IRealisateursFavorisUpdateService, RealisateursFavorisUpdateService>();
    }
}