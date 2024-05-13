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
    private const ushort NbBilletsMin = 1;
    private const ushort NbBilletsMax = 30;

    public Task ReserverProjection(Guid idProjection, ushort nbBillets = 1)
    {
        Guid idUtilisateur = utilisateurAuthenticationService.ObtenirIdUtilisateurConnecte();
        return AjouterBilletPourUsager(idProjection, idUtilisateur, nbBillets);
    }

    public Task OffrirBilletGratuit(Guid idProjection, Guid idUtilisateur)
    {
        return AjouterBilletPourUsager(idProjection, idUtilisateur);
    }

    private async Task AjouterBilletPourUsager(Guid idProjection, Guid idUtilisateur, ushort nbBillets = 1)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IUtilisateur utilisateur = await unitOfWork.UtilisateurRepository.ObtenirParIdAsync(idUtilisateur) ??
                                   throw new KeyNotFoundException("L'utilisateur est introuvable");
        IProjection projection = await unitOfWork.ProjectionRepository.ObtenirParIdAsync(idProjection) ??
                                 throw new KeyNotFoundException("La projection est introuvable");
        ISalle salle = await unitOfWork.SalleRepository.ObtenirParIdAsync(projection.IdSalle) ??
                       throw new KeyNotFoundException("La salle est introuvable");

        EffectuerValidations(unitOfWork, salle, projection, nbBillets);

        for (ushort i = 0; i < nbBillets; i++)
        {
            IBillet billet = new Billet(idProjection, idUtilisateur);
            await unitOfWork.BilletRepository.AjouterAsync(billet);

            utilisateur.AddBillets([billet.Id]);
            unitOfWork.UtilisateurRepository.Modifier(utilisateur);
        }

        await unitOfWork.SauvegarderAsync();
    }

    private static void EffectuerValidations(IUnitOfWork unitOfWork, ISalle salle, IProjection projection,
        ushort nbBillets)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderQteBillets(nbBillets),
            ValiderCapaciteSalle(unitOfWork, salle, projection, nbBillets)
        );
    }

    private static IEnumerable<ArgumentOutOfRangeException> ValiderQteBillets(ushort nbBillets)
    {
        if (nbBillets is < NbBilletsMin or > NbBilletsMax)
        {
            yield return new ArgumentOutOfRangeException(nameof(nbBillets),
                $"Le nombre de billets doit être compris entre {NbBilletsMin} et {NbBilletsMax}.");
        }
    }

    private static async IAsyncEnumerable<ArgumentOutOfRangeException> ValiderCapaciteSalle(IUnitOfWork unitOfWork,
        ISalle salle, IProjection projection, ushort nbBillets)
    {
        int nbBilletsEnCirculation =
            await unitOfWork.BilletRepository.CompterAsync(b => b.IdProjection == projection.Id);

        if (nbBilletsEnCirculation == salle.NbSieges)
        {
            yield return new ArgumentOutOfRangeException(nameof(projection),
                "Plus aucune place n'est disponible pour cette représentation.");
        }
        else if (nbBilletsEnCirculation + nbBillets > salle.NbSieges)
        {
            int deltaPlaces = nbBilletsEnCirculation + nbBillets - salle.NbSieges;
            yield return new ArgumentOutOfRangeException(nameof(projection),
                $"Impossible de compléter l'opération puisqu'il manque {deltaPlaces} places dans la salle.");
        }
    }
}