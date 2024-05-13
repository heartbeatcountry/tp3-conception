namespace CineQuebec.Application.Interfaces.Services.Preferences;

public interface INoteFilmCreationService
{
    Task<float> NoterFilm(Guid pIdFilm, byte pNote);
}