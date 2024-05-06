using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Services.Preferences;

public class CategoriesPrefereesQueryService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : ICategoriesPrefereesQueryService
{
    public Task<IEnumerable<CategorieFilmDto>> ObtenirCategoriesPreferees()
    {
        throw new NotImplementedException();
    }
}