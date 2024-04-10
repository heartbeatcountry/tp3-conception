using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services;

public class ProjectionCreationService(IUnitOfWorkFactory unitOfWorkFactory) : IProjectionCreationService
{
    public async Task<Guid> CreerProjection(Guid pFilm, Guid pSalle, DateTime pDateHeure, bool pEstAvantPremiere)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IEnumerable<Exception> exceptions = await EffectuerValidations(unitOfWork, pFilm, pSalle, pDateHeure,
            pEstAvantPremiere);

        if (exceptions.ToArray() is { Length: > 0 } innerExceptions)
        {
            throw new AggregateException("Des erreurs se sont produites lors de la validation des données.",
                innerExceptions);
        }

        Projection projection = new Projection(pFilm, pSalle, pDateHeure, pEstAvantPremiere);
        IProjection projectionCreee = await unitOfWork.ProjectionRepository.AjouterAsync(projection);

        await unitOfWork.SauvegarderAsync();

        return projectionCreee.Id;
    }


    private static async Task<IEnumerable<Exception>> EffectuerValidations(IUnitOfWork unitOfWork, Guid pFilm,
        Guid pSalle, DateTime pDateHeure, bool pEstAvantPremiere)
    {
        List<Exception> exceptions = [];

        exceptions.AddRange(await ValiderFilmExiste(unitOfWork, pFilm));
        exceptions.AddRange(await ValiderSalleExiste(unitOfWork, pSalle));
        exceptions.AddRange(await ValiderSalleDispo(unitOfWork, pSalle, pDateHeure));
        exceptions.AddRange(await ValiderProjectionEstUnique(unitOfWork, pFilm, pSalle, pDateHeure));
        exceptions.AddRange(ValiderDateHeure(pDateHeure));

        return exceptions;
    }

    private static IEnumerable<Exception> ValiderDateHeure(DateTime pDateHeure)
    {
        List<Exception> exceptions = [];

        if (pDateHeure < DateTime.Now)
        {
            exceptions.Add(new ArgumentOutOfRangeException(nameof(pDateHeure),
                "La date de projection ne doit pas être dans le passé."));
        }

        return exceptions;
    }


    private static async Task<IEnumerable<Exception>> ValiderFilmExiste(IUnitOfWork unitOfWork, Guid pFilm)
    {
        List<Exception> exceptions = [];

        if (await unitOfWork.FilmRepository.ObtenirParIdAsync(pFilm) is null)
        {
            exceptions.Add(new ArgumentException($"Le film avec l'identifiant {pFilm} n'existe pas.",
                nameof(pFilm)));
        }

        return exceptions;
    }


    private static async Task<IEnumerable<Exception>> ValiderProjectionEstUnique(IUnitOfWork unitOfWork, Guid pFilm,
        Guid pSalle, DateTime pDateHeure)
    {
        List<Exception> exceptions = [];

        if (await unitOfWork.ProjectionRepository.ExisteAsync(f =>
                f.IdFilm == pFilm && f.IdSalle == pSalle && f.DateHeure == pDateHeure))
        {
            exceptions.Add(new ArgumentException(
                "Une projection avec le même film, la même date et heure dans la même salle existe déjà.",
                nameof(pFilm)));
        }

        return exceptions;
    }

    private static async Task<IEnumerable<Exception>> ValiderSalleDispo(IUnitOfWork unitOfWork, Guid pSalle,
        DateTime pDateHeure)
    {
        List<Exception> exceptions = [];

        if (await unitOfWork.ProjectionRepository.ExisteAsync(proj =>
                proj.IdSalle == pSalle && proj.DateHeure == pDateHeure))
        {
            exceptions.Add(new ArgumentException(
                $"La salle avec l'identifiant {pSalle} n'est pas disponible pour la date {pDateHeure}.",
                nameof(pSalle)));
        }

        return exceptions;
    }

    private static async Task<IEnumerable<Exception>> ValiderSalleExiste(IUnitOfWork unitOfWork, Guid pSalle)
    {
        List<Exception> exceptions = [];

        if (await unitOfWork.SalleRepository.ObtenirParIdAsync(pSalle) is null)
        {
            exceptions.Add(new ArgumentException($"La salle avec l'identifiant {pSalle} n'existe pas.",
                nameof(pSalle)));
        }

        return exceptions;
    }
}