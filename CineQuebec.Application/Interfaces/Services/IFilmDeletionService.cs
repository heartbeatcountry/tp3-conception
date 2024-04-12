namespace CineQuebec.Application.Interfaces.Services;

public interface IFilmDeletionService
{
    Task<bool> SupprimerFilm(Guid id);
}