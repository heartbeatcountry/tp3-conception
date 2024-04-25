using System.Security.Claims;

namespace CineQuebec.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<bool> AuthentifierThreadAsync(string nomUsager, string mdp);
    void DeauthentifierThread();
    ClaimsPrincipal? ObtenirAutorisation();
}