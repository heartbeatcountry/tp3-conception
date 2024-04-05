using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Services;

public class FilmQueryService(IUnitOfWorkFactory unitOfWorkFactory) : IFilmQueryService
{
	public async Task<IEnumerable<FilmDto>> GetFilms()
	{
		using var unitOfWork = unitOfWorkFactory.Create();
		var films = await unitOfWork.FilmRepository.ObtenirTousAsync();
		return films.Select(f => f.VersDto(Enumerable.Empty<CategorieFilmDto>(), Enumerable.Empty<RealisateurDto>(),
			Enumerable.Empty<ActeurDto>()));
	}
}