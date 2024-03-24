using CineQuebec.Application.Interfaces;
using CineQuebec.Domain.Entities.Films;

namespace CineQuebec.Application.Services;

public class FilmService(IUnitOfWork unitOfWork) : BaseService(unitOfWork)
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<IEnumerable<Film>> GetFilms()
	{
		return await _unitOfWork.FilmRepository.ObtenirTousAsync();
	}
}