using System.Security.Claims;

namespace CineQuebec.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task AuthentifierThreadAsync(string nomUsager, string mdp);
    void DeauthentifierThread();
    ClaimsPrincipal? ObtenirAutorisation();
}