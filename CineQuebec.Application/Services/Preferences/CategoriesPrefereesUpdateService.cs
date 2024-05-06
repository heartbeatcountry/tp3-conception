using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Identity;
using CineQuebec.Application.Interfaces.Services.Preferences;

namespace CineQuebec.Application.Services.Preferences;

public class CategoriesPrefereesUpdateService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IUtilisateurAuthenticationService utilisateurAuthenticationService) : ICategoriesPrefereesUpdateService
{
    public Task AjouterCategoriePreferee(Guid idCategorie)
    {
        throw new NotImplementedException();
    }

    public Task RetirerCategoriePreferee(Guid idCategorie)
    {
        throw new NotImplementedException();
    }
}