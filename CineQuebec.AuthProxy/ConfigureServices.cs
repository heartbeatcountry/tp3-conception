using CineQuebec.Application;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Persistence;

using Microsoft.Extensions.DependencyInjection;

namespace CineQuebec.AuthProxy;

public static class ConfigureServices
{
    public static IServiceCollection AddProxiedServices(this IServiceCollection services)
    {
        return services
            .AddPersistenceServices()
            .AddApplicationServices()
            .WrapServicesWithAuthProxy();
    }

    public static IServiceCollection WrapServicesWithAuthProxy(this IServiceCollection services)
    {
        return services
            .AjouterProxyAuthPourService<IActeurCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] = [nameof(IActeurCreationService.CreerActeur)]
            })
            .AjouterProxyAuthPourService<IActeurQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Invite] = [nameof(IActeurQueryService.ObtenirTous)]
            })
            .AjouterProxyAuthPourService<ICategorieFilmCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] = [nameof(ICategorieFilmCreationService.CreerCategorie)]
            })
            .AjouterProxyAuthPourService<ICategorieFilmQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Invite] = [nameof(ICategorieFilmQueryService.ObtenirToutes)]
            })
            .AjouterProxyAuthPourService<IFilmCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] = [nameof(IFilmCreationService.CreerFilm)]
            })
            .AjouterProxyAuthPourService<IFilmDeletionService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] = [nameof(IFilmDeletionService.SupprimerFilm)]
            })
            .AjouterProxyAuthPourService<IFilmQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Invite] =
                [
                    nameof(IFilmQueryService.ObtenirDetailsFilmParId),
                    nameof(IFilmQueryService.ObtenirTousAlAffiche)
                ],
                [Role.Administrateur] = [nameof(IFilmQueryService.ObtenirTous)]
            })
            .AjouterProxyAuthPourService<IFilmUpdateService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] = [nameof(IFilmUpdateService.ModifierFilm)]
            })
            .AjouterProxyAuthPourService<IProjectionCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] = [nameof(IProjectionCreationService.CreerProjection)]
            })
            .AjouterProxyAuthPourService<IProjectionDeletionService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] = [nameof(IProjectionDeletionService.SupprimerProjection)]
            })
            .AjouterProxyAuthPourService<IProjectionQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Invite] = [nameof(IProjectionQueryService.ObtenirProjectionsAVenirPourFilm)]
            })
            .AjouterProxyAuthPourService<IRealisateurCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] = [nameof(IRealisateurCreationService.CreerRealisateur)]
            })
            .AjouterProxyAuthPourService<IRealisateurQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Invite] = [nameof(IRealisateurQueryService.ObtenirTous)]
            })
            .AjouterProxyAuthPourService<ISalleCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] = [nameof(ISalleCreationService.CreerSalle)]
            })
            .AjouterProxyAuthPourService<ISalleQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] = [nameof(ISalleQueryService.ObtenirToutes)]
            })
            .AjouterProxyAuthPourService<INoteFilmCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Utilisateur] = [nameof(INoteFilmCreationService.NoterFilm)]
            });
    }
}