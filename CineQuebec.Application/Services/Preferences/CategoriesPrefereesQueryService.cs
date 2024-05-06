using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Services.Preferences;

public class CategoriesPrefereesQueryService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : ICategoriesPrefereesQueryService
{
    public async Task<IEnumerable<CategorieFilmDto>> ObtenirCategoriesPreferees()
    {
        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IUtilisateur utilisateur = await unitOfWork.UtilisateurRepository.ObtenirParIdAsync(idUtilisateur)
                                   ?? throw new InvalidOperationException("L'utilisateur n'existe plus");
        IEnumerable<ICategorieFilm> categoriefilms =
            await unitOfWork.CategorieFilmRepository.ObtenirParIdsAsync(utilisateur.CategoriesPrefereesParId);

        return categoriefilms.Select(categoriefilm => categoriefilm.VersDto());
    }
}