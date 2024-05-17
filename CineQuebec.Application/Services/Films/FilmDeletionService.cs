using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services.Films;

public class FilmDeletionService(IUnitOfWorkFactory unitOfWorkFactory) : IFilmDeletionService
{
    public async Task<bool> SupprimerFilm(Guid id)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IFilm? film = await unitOfWork.FilmRepository.ObtenirParIdAsync(id);

        if (film is null)
        {
            return false;
        }

        await SupprimerProjections(unitOfWork, id);
        await SupprimerEvaluations(unitOfWork, id);
        SupprimerFilm(unitOfWork, film);

        await unitOfWork.SauvegarderAsync();

        return true;
    }

    private static async Task SupprimerProjections(IUnitOfWork unitOfWork, Guid idFilm)
    {
        IEnumerable<IProjection> projections =
            await unitOfWork.ProjectionRepository.ObtenirTousAsync(pr => pr.IdFilm == idFilm);

        foreach (IProjection projection in projections)
        {
            await SupprimerBilletsDeProjection(unitOfWork, projection.Id);
            unitOfWork.ProjectionRepository.Supprimer(projection);
        }
    }

    private static async Task SupprimerBilletsDeProjection(IUnitOfWork unitOfWork, Guid idProjection)
    {
        IEnumerable<IBillet> billets =
            await unitOfWork.BilletRepository.ObtenirTousAsync(b => b.IdProjection == idProjection);

        foreach (IBillet billet in billets)
        {
            unitOfWork.BilletRepository.Supprimer(billet);
        }
    }

    private static async Task SupprimerEvaluations(IUnitOfWork unitOfWork, Guid idFilm)
    {
        IEnumerable<INoteFilm> evaluations =
            await unitOfWork.NoteFilmRepository.ObtenirTousAsync(e => e.IdFilm == idFilm);

        foreach (INoteFilm evaluation in evaluations)
        {
            unitOfWork.NoteFilmRepository.Supprimer(evaluation);
        }
    }

    private static void SupprimerFilm(IUnitOfWork unitOfWork, IFilm film)
    {
        unitOfWork.FilmRepository.Supprimer(film);
    }
}