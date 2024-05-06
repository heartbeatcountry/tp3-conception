namespace CineQuebec.Application.Interfaces.Services.Preferences;

public interface INoteFilmCreationService
{
    Task<Guid> NoterFilm(Guid pIdFilm, byte pNote);
}