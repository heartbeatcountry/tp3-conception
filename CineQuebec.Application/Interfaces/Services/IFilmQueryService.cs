using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Interfaces.Services;

public interface IFilmQueryService
{
	Task<IEnumerable<FilmDto>> GetFilms();
}