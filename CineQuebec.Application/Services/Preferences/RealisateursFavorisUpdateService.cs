using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;

namespace CineQuebec.Application.Services.Preferences;

public class RealisateursFavorisUpdateService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : IRealisateursFavorisUpdateService
{
    public Task AjouterRealisateurFavori(Guid idRealisateur)
    {
        throw new NotImplementedException();
    }

    public Task RetirerRealisateurFavori(Guid idRealisateur)
    {
        throw new NotImplementedException();
    }
}