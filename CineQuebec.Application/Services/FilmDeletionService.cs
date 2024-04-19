using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Interfaces.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services;

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
            unitOfWork.ProjectionRepository.Supprimer(projection);
        }
    }

    private static void SupprimerFilm(IUnitOfWork unitOfWork, IFilm film)
    {
        unitOfWork.FilmRepository.Supprimer(film);
    }
}