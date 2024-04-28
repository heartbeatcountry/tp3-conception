using CineQuebec.Application.Records.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Records.Films;

public record FilmDto(
    Guid Id,
    string Titre,
    string Description,
    CategorieFilmDto? Categorie,
    IEnumerable<RealisateurDto> Realisateurs,
    IEnumerable<ActeurDto> Acteurs,
    DateTime DateSortieInternationale,
    ushort DureeEnMinutes,
    ushort NoteMoyenne
    ) : EntityDto(Id);

internal static class FilmExtensions
{
    internal static FilmDto VersDto(this IFilm film, CategorieFilmDto? categorie,
        IEnumerable<RealisateurDto> realisateurs,
        IEnumerable<ActeurDto> acteurs)
    {
        return new FilmDto(film.Id, film.Titre, film.Description, categorie, realisateurs, acteurs,
            film.DateSortieInternationale,
            film.DureeEnMinutes, film.NoteMoyenne);
    }
}