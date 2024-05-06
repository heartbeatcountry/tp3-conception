using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Services.Preferences;

public class ActeursFavorisQueryService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : IActeursFavorisQueryService
{
    public async Task<IEnumerable<ActeurDto>> ObtenirActeursFavoris()
    {
        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IUtilisateur utilisateur = await unitOfWork.UtilisateurRepository.ObtenirParIdAsync(idUtilisateur)
                                   ?? throw new InvalidOperationException("L'utilisateur n'existe plus");
        IEnumerable<IActeur> acteurs =
            await unitOfWork.ActeurRepository.ObtenirParIdsAsync(utilisateur.ActeursFavorisParId);

        return acteurs.Select(acteur => acteur.VersDto());
    }
}