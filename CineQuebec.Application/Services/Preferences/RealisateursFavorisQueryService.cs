using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Services.Preferences;

public class RealisateursFavorisQueryService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : IRealisateursFavorisQueryService
{
    public async Task<IEnumerable<RealisateurDto>> ObtenirRealisateursFavoris()
    {
        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();

        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IUtilisateur utilisateur = await unitOfWork.UtilisateurRepository.ObtenirParIdAsync(idUtilisateur)
                                   ?? throw new InvalidOperationException("L'utilisateur n'existe plus");
        IEnumerable<IRealisateur> realisateurs =
            await unitOfWork.RealisateurRepository.ObtenirParIdsAsync(utilisateur.RealisateursFavorisParId);

        return realisateurs.Select(realisateur => realisateur.VersDto());
    }
}