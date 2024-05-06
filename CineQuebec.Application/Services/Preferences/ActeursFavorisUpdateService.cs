using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Services.Preferences;

public class ActeursFavorisUpdateService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService)
    : ServiceAvecValidation, IActeursFavorisUpdateService
{
    public async Task AjouterActeurFavori(Guid idActeur)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IUtilisateur utilisateur = await ObtenirUtilisateur(unitOfWork);

        if (utilisateur.ActeursFavorisParId.Contains(idActeur))
        {
            return;
        }

        EffectuerValidations(unitOfWork, idActeur);

        utilisateur.AddActeursFavoris([idActeur]);
        unitOfWork.UtilisateurRepository.Modifier(utilisateur);

        await unitOfWork.SauvegarderAsync();
    }

    public async Task RetirerActeurFavori(Guid idActeur)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IUtilisateur utilisateur = await ObtenirUtilisateur(unitOfWork);

        if (!utilisateur.ActeursFavorisParId.Contains(idActeur))
        {
            return;
        }

        utilisateur.RemoveActeursFavoris([idActeur]);
        unitOfWork.UtilisateurRepository.Modifier(utilisateur);

        await unitOfWork.SauvegarderAsync();
    }

    private async Task<IUtilisateur> ObtenirUtilisateur(IUnitOfWork unitOfWork)
    {
        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();
        return await unitOfWork.UtilisateurRepository.ObtenirParIdAsync(idUtilisateur)
               ?? throw new InvalidOperationException("L'utilisateur n'existe plus");
    }

    private static void EffectuerValidations(IUnitOfWork unitOfWork, Guid idActeur)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderActeurExiste(unitOfWork, idActeur)
        );
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderActeurExiste(IUnitOfWork unitOfWork, Guid idActeur)
    {
        if (!await unitOfWork.ActeurRepository.ExisteAsync(idActeur))
        {
            yield return new ArgumentException($"L'acteur avec l'identifiant {idActeur} n'existe pas.",
                nameof(idActeur));
        }
    }
}