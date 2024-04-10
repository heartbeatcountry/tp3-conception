using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Interfaces.Services;

public interface ICategorieFilmQueryService
{
    Task<IEnumerable<CategorieFilmDto>> ObtenirToutes();
}