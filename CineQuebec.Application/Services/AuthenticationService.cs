using System.Globalization;
using System.Security.Claims;

using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Utilisateurs;

namespace CineQuebec.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private const byte ExpirationEnHeures = 24;
    private ClaimsPrincipal? _cachedClaimsPrincipal;

    public ClaimsPrincipal? ObtenirAutorisation()
    {
        return ClaimsPrincipal.Current ?? _cachedClaimsPrincipal;
    }

    public async Task AuthentifierThreadAsync(string nomUsager, string mdp)
    {
        ClaimsPrincipal claimsPrincipal = CreerClaimsPrincipal(nomUsager, Role.Utilisateur, Role.Administrateur);
        Thread.CurrentPrincipal = _cachedClaimsPrincipal = claimsPrincipal;
    }

    public void DeauthentifierThread()
    {
        Thread.CurrentPrincipal = _cachedClaimsPrincipal = null;
    }

    private static ClaimsPrincipal CreerClaimsPrincipal(string nomUsager, params Role[] roles)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, nomUsager), new(ClaimTypes.Expiration,
                    DateTime.Now.AddHours(ExpirationEnHeures).ToString("O", CultureInfo.InvariantCulture))
            }.Union(roles.Select(role => new Claim(ClaimTypes.Role, role.ToString()))),
            "Basic"));
    }
}