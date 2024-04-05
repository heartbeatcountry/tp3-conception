using CineQuebec.Application.Records.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Records.Films;

public record class FilmDto(
	Guid Id,
	string Titre,
	string Description,
	IEnumerable<CategorieFilmDto> Categories,
	IEnumerable<RealisateurDto> Realisateurs,
	IEnumerable<ActeurDto> Acteurs,
	DateTime DateSortieInternationale,
	ushort DureeEnMinutes) : EntityDto(Id);

internal static class FilmExtensions
{
	internal static FilmDto VersDto(this IFilm film, IEnumerable<CategorieFilmDto> categories,
		IEnumerable<RealisateurDto> realisateurs,
		IEnumerable<ActeurDto> acteurs)
	{
		return new FilmDto(film.Id, film.Titre, film.Description, categories, realisateurs, acteurs,
			film.DateSortieInternationale,
			film.DureeEnMinutes);
	}
}