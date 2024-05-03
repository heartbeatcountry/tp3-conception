using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Interfaces.Services.Films;

public interface IFilmQueryService
{
    Task<IEnumerable<FilmDto>> ObtenirTous();
    Task<IEnumerable<FilmDto>> ObtenirTousAlAffiche();
    Task<FilmDto?> ObtenirDetailsFilmParId(Guid id);
    Task<IEnumerable<FilmDto>> ObtenirFilmsAssistesParUtilisateur();
}