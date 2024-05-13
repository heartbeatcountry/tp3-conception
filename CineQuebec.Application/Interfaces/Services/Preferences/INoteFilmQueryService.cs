namespace CineQuebec.Application.Interfaces.Services.Preferences;

public interface INoteFilmQueryService
{
    Task<byte?> ObtenirMaNotePourFilm(Guid pIdFilm);
}