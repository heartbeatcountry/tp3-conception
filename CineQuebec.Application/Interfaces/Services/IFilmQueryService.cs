using CineQuebec.Domain.Entities.Films;

namespace CineQuebec.Application.Interfaces.Services;

public interface IFilmQueryService
{
	Task<IEnumerable<Film>> GetFilms();
}