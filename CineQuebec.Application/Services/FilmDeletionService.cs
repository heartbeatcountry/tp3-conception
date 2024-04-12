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

        var projections = await unitOfWork.ProjectionRepository.ObtenirTousAsync(pr => pr.IdFilm == id);

        foreach (IProjection projection in projections)
        {
            unitOfWork.ProjectionRepository.Supprimer(projection);
        }

        unitOfWork.FilmRepository.Supprimer(film);
        await unitOfWork.SauvegarderAsync();

        return true;
    }
}