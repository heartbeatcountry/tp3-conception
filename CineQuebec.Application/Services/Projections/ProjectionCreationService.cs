using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services.Projections;

public class ProjectionCreationService(IUnitOfWorkFactory unitOfWorkFactory)
    : ServiceAvecValidation, IProjectionCreationService
{
    public async Task<Guid> CreerProjection(Guid pFilm, Guid pSalle, DateTime pDateHeure, bool pEstAvantPremiere)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        EffectuerValidations(unitOfWork, pFilm, pSalle, pDateHeure, pEstAvantPremiere);

        IProjection projectionCreee =
            await CreerNouvProjection(unitOfWork, pFilm, pSalle, pDateHeure, pEstAvantPremiere);

        await unitOfWork.SauvegarderAsync();

        return projectionCreee.Id;
    }


    private static void EffectuerValidations(IUnitOfWork unitOfWork, Guid pFilm,
        Guid pSalle, DateTime pDateHeure, bool pEstAvantPremiere)
    {
        LeverAggregateExceptionAuBesoin(
            ValiderFilmExiste(unitOfWork, pFilm),
            ValiderSalleExiste(unitOfWork, pSalle),
            ValiderSalleDispo(unitOfWork, pSalle, pDateHeure),
            ValiderProjectionEstUnique(unitOfWork, pFilm, pSalle, pDateHeure),
            ValiderAvantPremiereAvantAutresProjections(unitOfWork, pFilm, pDateHeure, pEstAvantPremiere),
            ValiderDateHeure(pDateHeure)
        );
    }

    private static async Task<IProjection> CreerNouvProjection(IUnitOfWork unitOfWork, Guid pFilm,
        Guid pSalle, DateTime pDateHeure, bool pEstAvantPremiere)
    {
        IProjection projection = new Projection(pFilm, pSalle, pDateHeure, pEstAvantPremiere);

        return await unitOfWork.ProjectionRepository.AjouterAsync(projection);
    }

    private static IEnumerable<ArgumentOutOfRangeException> ValiderDateHeure(DateTime pDateHeure)
    {
        if (pDateHeure < DateTime.Now)
        {
            yield return new ArgumentOutOfRangeException(nameof(pDateHeure),
                "La date de projection ne doit pas être dans le passé.");
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderFilmExiste(IUnitOfWork unitOfWork, Guid pFilm)
    {
        if (await unitOfWork.FilmRepository.ObtenirParIdAsync(pFilm) is null)
        {
            yield return new ArgumentException($"Le film avec l'identifiant {pFilm} n'existe pas.",
                nameof(pFilm));
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderProjectionEstUnique(IUnitOfWork unitOfWork,
        Guid pFilm,
        Guid pSalle, DateTime pDateHeure)
    {
        if (await unitOfWork.ProjectionRepository.ExisteAsync(f =>
                f.IdFilm == pFilm && f.IdSalle == pSalle && f.DateHeure == pDateHeure))
        {
            yield return new ArgumentException(
                "Une projection avec le même film, la même date et heure dans la même salle existe déjà.",
                nameof(pFilm));
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderSalleDispo(IUnitOfWork unitOfWork, Guid pSalle,
        DateTime pDateHeure)
    {
        if (await unitOfWork.ProjectionRepository.ExisteAsync(proj =>
                proj.IdSalle == pSalle && proj.DateHeure == pDateHeure))
        {
            yield return new ArgumentException(
                $"La salle avec l'identifiant {pSalle} n'est pas disponible pour la date {pDateHeure}.",
                nameof(pSalle));
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderSalleExiste(IUnitOfWork unitOfWork, Guid pSalle)
    {
        if (await unitOfWork.SalleRepository.ObtenirParIdAsync(pSalle) is null)
        {
            yield return new ArgumentException($"La salle avec l'identifiant {pSalle} n'existe pas.",
                nameof(pSalle));
        }
    }

    private static async IAsyncEnumerable<ArgumentException> ValiderAvantPremiereAvantAutresProjections(
        IUnitOfWork unitOfWork, Guid pFilm, DateTime pDateHeure, bool pEstAvantPremiere)
    {
        if (pEstAvantPremiere && await unitOfWork.ProjectionRepository.ExisteAsync(proj =>
                proj.IdFilm == pFilm && proj.DateHeure < pDateHeure))
        {
            yield return new ArgumentException(
                "Une projection régulière du film existe déjà avant la date de la projection avant-première.",
                nameof(pFilm));
        }
    }
}