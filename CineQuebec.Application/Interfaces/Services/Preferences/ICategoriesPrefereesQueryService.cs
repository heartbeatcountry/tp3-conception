using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Interfaces.Services.Preferences;

public interface ICategoriesPrefereesQueryService
{
    Task<IEnumerable<CategorieFilmDto>> ObtenirCategoriesPreferees();
}