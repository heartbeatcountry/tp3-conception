namespace CineQuebec.Application.Interfaces.Services.Films;

public interface IFilmDeletionService
{
    Task<bool> SupprimerFilm(Guid id);
}