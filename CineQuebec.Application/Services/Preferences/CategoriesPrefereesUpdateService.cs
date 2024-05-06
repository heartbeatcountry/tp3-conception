using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Services.Preferences;

public class CategoriesPrefereesUpdateService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService)
    : ServiceAvecValidation, ICategoriesPrefereesUpdateService
{
    public async Task AjouterCategoriePreferee(Guid idCategorie)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IUtilisateur utilisateur = await ObtenirUtilisateur(unitOfWork);

        if (utilisateur.CategoriesPrefereesParId.Contains(idCategorie))
        {
            return;
        }

        EffectuerValidations(unitOfWork, idCategorie);

        utilisateur.AddCategoriesPreferees([idCategorie]);
        unitOfWork.UtilisateurRepository.Modifier(utilisateur);

        await unitOfWork.SauvegarderAsync();
    }

    public async Task RetirerCategoriePreferee(Guid idCategorie)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IUtilisateur utilisateur = await ObtenirUtilisateur(unitOfWork);

        if (!utilisateur.CategoriesPrefereesParId.Contains(idCategorie))
        {
            return;
        }

        utilisateur.RemoveCategoriesPreferees([idCategorie]);
        unitOfWork.UtilisateurRepository.Modifier(utilisateur);

        await unitOfWork.SauvegarderAsync();
    }

    private async Task<IUtilisateur> ObtenirUtilisateur(IUnitOfWork unitOfWork)
    {
        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();
        return await unitOfWork.UtilisateurRepository.ObtenirParIdAsync(idUtilisateur)
               ?? throw new InvalidOperationException("L'utilisateur n'existe plus");
    }

    private static void EffectuerValidations(IUnitOfWork unitOfWork, Guid idCategorie)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderCategorieExiste(unitOfWork, idCategorie)
        );
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderCategorieExiste(IUnitOfWork unitOfWork,
        Guid idCategorie)
    {
        if (!await unitOfWork.CategorieFilmRepository.ExisteAsync(idCategorie))
        {
            yield return new ArgumentException($"La catégorie avec l'identifiant {idCategorie} n'existe pas.",
                nameof(idCategorie));
        }
    }
}