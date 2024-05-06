using System.Security.Claims;

namespace CineQuebec.Application.Interfaces.Services.Identity;

public interface IUtilisateurAuthenticationService
{
    Task AuthentifierThreadAsync(string courriel, string mdp);
    void DeauthentifierThread();
    ClaimsPrincipal? ObtenirAutorisation();
    Guid ObtenirIdUtilisateurConnecte();
}