using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Services;

public class FilmQueryService(IUnitOfWorkFactory unitOfWorkFactory) : IFilmQueryService
{
	public async Task<IEnumerable<FilmDto>> ObtenirTous()
	{
		using var unitOfWork = unitOfWorkFactory.Create();
		var films = await unitOfWork.FilmRepository.ObtenirTousAsync();
		return films.Select(f => f.VersDto(null, Enumerable.Empty<RealisateurDto>(),
			Enumerable.Empty<ActeurDto>()));
	}

    public async Task<FilmDto?> ObtenirDetailsFilmParId(Guid id)
    {
        using var unitOfWork = unitOfWorkFactory.Create();
        var film = await unitOfWork.FilmRepository.ObtenirParIdAsync(id);

        if (film is null)
        {
            return null;
        }

        var categorie = await unitOfWork.CategorieFilmRepository.ObtenirParIdAsync(film.IdCategorie);
        var realisateurs = await unitOfWork.RealisateurRepository.ObtenirParIdsAsync(film.RealisateursParId);
        var acteurs = await unitOfWork.ActeurRepository.ObtenirParIdsAsync(film.ActeursParId);

        var filmDto = film.VersDto(categorie?.VersDto(), realisateurs.Select(r => r.VersDto()),
            acteurs.Select(a => a.VersDto()));

        return filmDto;
    }
}