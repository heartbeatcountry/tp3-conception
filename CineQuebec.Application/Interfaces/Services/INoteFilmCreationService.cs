namespace CineQuebec.Application.Interfaces.Services;

public interface INoteFilmCreationService
{
    Task<Guid> NoterFilm(Guid pIdFilm, byte pNote);
}