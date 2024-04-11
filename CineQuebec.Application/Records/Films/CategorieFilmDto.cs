using CineQuebec.Application.Records.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Records.Films;

public record class CategorieFilmDto(Guid Id, string NomAffichage) : EntityDto(Id);

internal static class CategorieFilmExtensions
{
    internal static CategorieFilmDto VersDto(this ICategorieFilm categorieFilm)
    {
        return new CategorieFilmDto(categorieFilm.Id, categorieFilm.NomAffichage);
    }
}