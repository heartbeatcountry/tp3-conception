using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;
using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Services.Preferences;

public class RealisateursFavorisQueryService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : IRealisateursFavorisQueryService
{
    public Task<IEnumerable<RealisateurDto>> ObtenirRealisateursFavoris()
    {
        throw new NotImplementedException();
    }
}