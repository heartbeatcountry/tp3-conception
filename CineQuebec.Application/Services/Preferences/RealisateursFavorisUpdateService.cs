using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Services.Preferences;

public class RealisateursFavorisUpdateService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService)
    : ServiceAvecValidation, IRealisateursFavorisUpdateService
{
    public async Task AjouterRealisateurFavori(Guid idRealisateur)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IUtilisateur utilisateur = await ObtenirUtilisateur(unitOfWork);

        if (utilisateur.RealisateursFavorisParId.Contains(idRealisateur))
        {
            return;
        }

        EffectuerValidations(unitOfWork, idRealisateur);

        utilisateur.AddRealisateursFavoris([idRealisateur]);
        unitOfWork.UtilisateurRepository.Modifier(utilisateur);

        await unitOfWork.SauvegarderAsync();
    }

    public async Task RetirerRealisateurFavori(Guid idRealisateur)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IUtilisateur utilisateur = await ObtenirUtilisateur(unitOfWork);

        if (!utilisateur.RealisateursFavorisParId.Contains(idRealisateur))
        {
            return;
        }

        utilisateur.RemoveRealisateursFavoris([idRealisateur]);
        unitOfWork.UtilisateurRepository.Modifier(utilisateur);

        await unitOfWork.SauvegarderAsync();
    }

    private async Task<IUtilisateur> ObtenirUtilisateur(IUnitOfWork unitOfWork)
    {
        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();
        return await unitOfWork.UtilisateurRepository.ObtenirParIdAsync(idUtilisateur)
               ?? throw new InvalidOperationException("L'utilisateur n'existe plus");
    }

    private static void EffectuerValidations(IUnitOfWork unitOfWork, Guid idRealisateur)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderRealisateurExiste(unitOfWork, idRealisateur)
        );
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderRealisateurExiste(IUnitOfWork unitOfWork,
        Guid idRealisateur)
    {
        if (!await unitOfWork.RealisateurRepository.ExisteAsync(idRealisateur))
        {
            yield return new ArgumentException($"Le réalisateur avec l'identifiant {idRealisateur} n'existe pas.",
                nameof(idRealisateur));
        }
    }
}