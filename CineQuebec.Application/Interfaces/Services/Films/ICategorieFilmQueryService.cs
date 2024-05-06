using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Interfaces.Services.Films;

public interface ICategorieFilmQueryService
{
    Task<IEnumerable<CategorieFilmDto>> ObtenirToutes();
}