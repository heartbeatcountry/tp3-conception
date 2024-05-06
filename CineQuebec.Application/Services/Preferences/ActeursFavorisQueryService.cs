using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Services.Preferences;

public class ActeursFavorisQueryService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : IActeursFavorisQueryService
{
    public Task<IEnumerable<ActeurDto>> ObtenirActeursFavoris()
    {
        throw new NotImplementedException();
    }
}