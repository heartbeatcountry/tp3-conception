using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Services.Abstract;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services;

public class ProjectionCreationService(IUnitOfWorkFactory unitOfWorkFactory)
    : ServiceAvecValidation, IProjectionCreationService
{
    public async Task<Guid> CreerProjection(Guid pFilm, Guid pSalle, DateTime pDateHeure, bool pEstAvantPremiere)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        await EffectuerValidations(unitOfWork, pFilm, pSalle, pDateHeure);

        IProjection projectionCreee =
            await CreerNouvProjection(unitOfWork, pFilm, pSalle, pDateHeure, pEstAvantPremiere);

        await unitOfWork.SauvegarderAsync();

        return projectionCreee.Id;
    }


    private static async Task EffectuerValidations(IUnitOfWork unitOfWork, Guid pFilm,
        Guid pSalle, DateTime pDateHeure)
    {
        LeverAggregateExceptionAuBesoin(
            await ValiderFilmExiste(unitOfWork, pFilm),
            await ValiderSalleExiste(unitOfWork, pSalle),
            await ValiderSalleDispo(unitOfWork, pSalle, pDateHeure),
            await ValiderProjectionEstUnique(unitOfWork, pFilm, pSalle, pDateHeure),
            ValiderDateHeure(pDateHeure)
        );
    }

    private static async Task<IProjection> CreerNouvProjection(IUnitOfWork unitOfWork, Guid pFilm,
        Guid pSalle, DateTime pDateHeure, bool pEstAvantPremiere)
    {
        IProjection projection = new Projection(pFilm, pSalle, pDateHeure, pEstAvantPremiere);

        return await unitOfWork.ProjectionRepository.AjouterAsync(projection);
    }

    private static ArgumentOutOfRangeException? ValiderDateHeure(DateTime pDateHeure)
    {
        return pDateHeure < DateTime.Now
            ? new ArgumentOutOfRangeException(nameof(pDateHeure),
                "La date de projection ne doit pas être dans le passé.")
            : null;
    }


    private static async Task<ArgumentException?> ValiderFilmExiste(IUnitOfWork unitOfWork, Guid pFilm)
    {
        return await unitOfWork.FilmRepository.ObtenirParIdAsync(pFilm) is null
            ? new ArgumentException($"Le film avec l'identifiant {pFilm} n'existe pas.",
                nameof(pFilm))
            : null;
    }


    private static async Task<ArgumentException?> ValiderProjectionEstUnique(IUnitOfWork unitOfWork, Guid pFilm,
        Guid pSalle, DateTime pDateHeure)
    {
        if (await unitOfWork.ProjectionRepository.ExisteAsync(f =>
                f.IdFilm == pFilm && f.IdSalle == pSalle && f.DateHeure == pDateHeure))
        {
            return new ArgumentException(
                "Une projection avec le même film, la même date et heure dans la même salle existe déjà.",
                nameof(pFilm));
        }

        return null;
    }

    private static async Task<ArgumentException?> ValiderSalleDispo(IUnitOfWork unitOfWork, Guid pSalle,
        DateTime pDateHeure)
    {
        if (await unitOfWork.ProjectionRepository.ExisteAsync(proj =>
                proj.IdSalle == pSalle && proj.DateHeure == pDateHeure))
        {
            return new ArgumentException(
                $"La salle avec l'identifiant {pSalle} n'est pas disponible pour la date {pDateHeure}.",
                nameof(pSalle));
        }

        return null;
    }

    private static async Task<ArgumentException?> ValiderSalleExiste(IUnitOfWork unitOfWork, Guid pSalle)
    {
        if (await unitOfWork.SalleRepository.ObtenirParIdAsync(pSalle) is null)
        {
            return new ArgumentException($"La salle avec l'identifiant {pSalle} n'existe pas.",
                nameof(pSalle));
        }

        return null;
    }
}