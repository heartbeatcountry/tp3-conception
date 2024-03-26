using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Films;

namespace CineQuebec.Application.Services;

public class FilmQueryService(IUnitOfWork unitOfWork) : IFilmQueryService
{
	public async Task<IEnumerable<Film>> GetFilms()
	{
		return await unitOfWork.FilmRepository.ObtenirTousAsync();
	}
}