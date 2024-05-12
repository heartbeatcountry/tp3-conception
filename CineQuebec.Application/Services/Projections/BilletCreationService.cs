using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Services.Projections;

public class BilletCreationService(
    IUtilisateurAuthenticationService utilisateurAuthenticationService,
    IUnitOfWorkFactory unitOfWorkFactory) : ServiceAvecValidation, IBilletCreationService
{
    public Task ReserverProjection(Guid idProjection)
    {
        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();
        return AjouterBilletPourUsager(idProjection, idUtilisateur);
    }

    public Task OffrirBilletGratuit(Guid idProjection, Guid idUtilisateur)
    {
        return AjouterBilletPourUsager(idProjection, idUtilisateur);
    }

    private async Task AjouterBilletPourUsager(Guid idProjection, Guid idUtilisateur)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IUtilisateur utilisateur = await unitOfWork.UtilisateurRepository.ObtenirParIdAsync(idUtilisateur) ??
                                   throw new KeyNotFoundException("L'utilisateur est introuvable");
        IProjection projection = await unitOfWork.ProjectionRepository.ObtenirParIdAsync(idProjection) ??
                                 throw new KeyNotFoundException("La projection est introuvable");
        ISalle salle = await unitOfWork.SalleRepository.ObtenirParIdAsync(projection.IdSalle) ??
                       throw new KeyNotFoundException("La salle est introuvable");

        EffectuerValidations(unitOfWork, salle, projection);

        IBillet billet = new Billet(idUtilisateur, idProjection);
        await unitOfWork.BilletRepository.AjouterAsync(billet);

        utilisateur.AddBillets([billet.Id]);
        unitOfWork.UtilisateurRepository.Modifier(utilisateur);

        await unitOfWork.SauvegarderAsync();
    }

    private static void EffectuerValidations(IUnitOfWork unitOfWork, ISalle salle, IProjection projection)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderCapaciteSalle(unitOfWork, salle, projection)
        );
    }

    private static async IAsyncEnumerable<ArgumentOutOfRangeException> ValiderCapaciteSalle(IUnitOfWork unitOfWork,
        ISalle salle, IProjection projection)
    {
        int nbBilletsEnCirculation =
            await unitOfWork.BilletRepository.CompterAsync(b => b.IdProjection == projection.Id);

        if (nbBilletsEnCirculation == salle.NbSieges)
        {
            yield return new ArgumentOutOfRangeException(nameof(projection),
                "Plus aucune place n'est disponible pour cette représentation.");
        }
    }
}