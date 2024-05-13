using CineQuebec.Application;
using CineQuebec.Application.Interfaces.Services.Fidelite;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Persistence;

using Microsoft.Extensions.DependencyInjection;

namespace CineQuebec.AuthProxy;

public static class ConfigureServices
{
    internal static readonly HashSet<Type> ServicesAutorisesSansProxy =
    [
        // Les services qui sont utilisés par le proxy d'authentification
        // lui-même ne peuvent pas être interceptés par le proxy. Ils ont
        // donc une autorisation explicite pour contourner le proxy.

        typeof(IUtilisateurAuthenticationService),
        typeof(IPasswordHashingService),
        typeof(IPasswordValidationService)
    ];

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
            .AjouterProxyAuthPourService<IUtilisateurCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Invite] =
                [
                    nameof(IUtilisateurCreationService.CreerUtilisateurAsync)
                ]
            })
            .AjouterProxyAuthPourService<IActeurCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] =
                [
                    nameof(IActeurCreationService.CreerActeur)
                ]
            })
            .AjouterProxyAuthPourService<IActeurQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Invite] =
                [
                    nameof(IActeurQueryService.ObtenirTous)
                ]
            })
            .AjouterProxyAuthPourService<ICategorieFilmCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] =
                [
                    nameof(ICategorieFilmCreationService.CreerCategorie)
                ]
            })
            .AjouterProxyAuthPourService<ICategorieFilmQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Invite] =
                [
                    nameof(ICategorieFilmQueryService.ObtenirToutes)
                ]
            })
            .AjouterProxyAuthPourService<IFilmCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] =
                [
                    nameof(IFilmCreationService.CreerFilm)
                ]
            })
            .AjouterProxyAuthPourService<IFilmDeletionService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] =
                [
                    nameof(IFilmDeletionService.SupprimerFilm)
                ]
            })
            .AjouterProxyAuthPourService<IFilmQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Invite] =
                [
                    nameof(IFilmQueryService.ObtenirDetailsFilmParId),
                    nameof(IFilmQueryService.ObtenirTousAlAffiche),
                    nameof(IFilmQueryService.ObtenirTous)
                ],
                [Role.Utilisateur] =
                [
                    nameof(IFilmQueryService.ObtenirFilmsAssistesParUtilisateur)
                ]
            })
            .AjouterProxyAuthPourService<IFilmUpdateService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] =
                [
                    nameof(IFilmUpdateService.ModifierFilm)
                ]
            })
            .AjouterProxyAuthPourService<IProjectionCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] =
                [
                    nameof(IProjectionCreationService.CreerProjection)
                ]
            })
            .AjouterProxyAuthPourService<IProjectionDeletionService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] =
                [
                    nameof(IProjectionDeletionService.SupprimerProjection)
                ]
            })
            .AjouterProxyAuthPourService<IProjectionQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Invite] =
                [
                    nameof(IProjectionQueryService.ObtenirProjectionsAVenirPourFilm)
                ]
            })
            .AjouterProxyAuthPourService<IBilletCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Utilisateur] =
                [
                    nameof(IBilletCreationService.ReserverProjection)
                ],
                [Role.Administrateur] =
                [
                    nameof(IBilletCreationService.OffrirBilletGratuit)
                ]
            })
            .AjouterProxyAuthPourService<IRealisateurCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] =
                [
                    nameof(IRealisateurCreationService.CreerRealisateur)
                ]
            })
            .AjouterProxyAuthPourService<IRealisateurQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Invite] =
                [
                    nameof(IRealisateurQueryService.ObtenirTous)
                ]
            })
            .AjouterProxyAuthPourService<ISalleCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] =
                [
                    nameof(ISalleCreationService.CreerSalle)
                ]
            })
            .AjouterProxyAuthPourService<ISalleQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] =
                [
                    nameof(ISalleQueryService.ObtenirToutes)
                ]
            })
            .AjouterProxyAuthPourService<INoteFilmCreationService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Utilisateur] =
                [
                    nameof(INoteFilmCreationService.NoterFilm)
                ]
            })
            .AjouterProxyAuthPourService<ICategoriesPrefereesQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Utilisateur] =
                [
                    nameof(ICategoriesPrefereesQueryService.ObtenirCategoriesPreferees)
                ]
            })
            .AjouterProxyAuthPourService<ICategoriesPrefereesUpdateService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Utilisateur] =
                [
                    nameof(ICategoriesPrefereesUpdateService.AjouterCategoriePreferee),
                    nameof(ICategoriesPrefereesUpdateService.RetirerCategoriePreferee)
                ]
            })
            .AjouterProxyAuthPourService<IActeursFavorisQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Utilisateur] =
                [
                    nameof(IActeursFavorisQueryService.ObtenirActeursFavoris)
                ]
            })
            .AjouterProxyAuthPourService<IActeursFavorisUpdateService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Utilisateur] =
                [
                    nameof(IActeursFavorisUpdateService.AjouterActeurFavori),
                    nameof(IActeursFavorisUpdateService.RetirerActeurFavori)
                ]
            })
            .AjouterProxyAuthPourService<IRealisateursFavorisQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Utilisateur] =
                [
                    nameof(IRealisateursFavorisQueryService.ObtenirRealisateursFavoris)
                ]
            })
            .AjouterProxyAuthPourService<IRealisateursFavorisUpdateService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Utilisateur] =
                [
                    nameof(IRealisateursFavorisUpdateService.AjouterRealisateurFavori),
                    nameof(IRealisateursFavorisUpdateService.RetirerRealisateurFavori)
                ]
            })
            .AjouterProxyAuthPourService<IUtilisateurFideliteQueryService>(new Dictionary<Role, IEnumerable<string>>
            {
                [Role.Administrateur] =
                [
                    nameof(IUtilisateurFideliteQueryService.ObtenirUtilisateursFideles)
                ]
            })
            .BloquerToutAutreServiceSansAutorisationExplicite();
    }
}