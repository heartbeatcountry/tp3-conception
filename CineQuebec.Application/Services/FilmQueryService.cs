using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Films;

namespace CineQuebec.Application.Services;

public class FilmQueryService(IUnitOfWorkFactory unitOfWorkFactory) : IFilmQueryService
{
	public async Task<IEnumerable<Film>> GetFilms()
	{
		using var unitOfWork = unitOfWorkFactory.Create();
		return await unitOfWork.FilmRepository.ObtenirTousAsync();
	}
}