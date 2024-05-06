using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;

namespace CineQuebec.Application.Services.Preferences;

public class ActeursFavorisUpdateService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : IActeursFavorisUpdateService
{
    public Task AjouterActeurFavori(Guid idActeur)
    {
        throw new NotImplementedException();
    }

    public Task RetirerActeurFavori(Guid idActeur)
    {
        throw new NotImplementedException();
    }
}