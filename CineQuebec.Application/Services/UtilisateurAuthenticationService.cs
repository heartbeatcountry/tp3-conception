using System.Globalization;
using System.Security;
using System.Security.Claims;

using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Services;

public class UtilisateurAuthenticationService(
    IUnitOfWorkFactory unitOfWorkFactory,
    IPasswordHashingService passwordHashingService) : IUtilisateurAuthenticationService
{
    private const byte ExpirationEnHeures = 24;
    private ClaimsPrincipal? _cachedClaimsPrincipal;

    public ClaimsPrincipal? ObtenirAutorisation()
    {
        return ClaimsPrincipal.Current ?? _cachedClaimsPrincipal;
    }

    public Guid ObtenirIdUtilisateur()
    {
        var utilisateur = ObtenirAutorisation();

        if (utilisateur  == null)
        {
            throw new SecurityException("L'utilisateur doit être authentifié");
        }

        return Guid.Parse(utilisateur.Claims.First(c => c.Type == ClaimTypes.PrimarySid).Value);
    }

    public async Task AuthentifierThreadAsync(string courriel, string mdp)
    {
        if (string.IsNullOrWhiteSpace(courriel))
        {
            throw new ArgumentNullException(nameof(courriel), "L'adresse courriel ne doit pas être vide.");
        }

        if (string.IsNullOrWhiteSpace(mdp))
        {
            throw new ArgumentNullException(nameof(mdp), "Le mot de passe ne doit pas être vide.");
        }

        courriel = courriel.Trim().ToLowerInvariant();
        mdp = mdp.Trim();

        IUtilisateur utilisateur = await ObtenirUtilisateurAsync(courriel);

        ValiderMdp(mdp, utilisateur.HashMotDePasse);
        await RehacherAuBesoin(utilisateur, mdp);

        Thread.CurrentPrincipal = _cachedClaimsPrincipal =
            CreerClaimsPrincipal(utilisateur.Id, utilisateur.Courriel, utilisateur.Roles.ToArray());
    }

    public void DeauthentifierThread()
    {
        Thread.CurrentPrincipal = _cachedClaimsPrincipal = null;
    }

    private async Task<IUtilisateur> ObtenirUtilisateurAsync(string courriel)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IUtilisateur? utilisateur = await unitOfWork.UtilisateurRepository.ObtenirAsync(iu => iu.Courriel == courriel);

        return utilisateur ??
               throw new KeyNotFoundException("Nom d'utilisateur ou mot de passe invalide.");
    }

    private void ValiderMdp(string mdp, string hash)
    {
        if (!passwordHashingService.ValiderMdp(mdp, hash))
        {
            throw new KeyNotFoundException("Nom d'utilisateur ou mot de passe invalide.");
        }
    }

    private async Task RehacherAuBesoin(IUtilisateur utilisateur, string mdp)
    {
        if (passwordHashingService.DoitEtreRehache(utilisateur.HashMotDePasse))
        {
            utilisateur.SetHashMotDePasse(passwordHashingService.HacherMdp(mdp));
            using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
            unitOfWork.UtilisateurRepository.Modifier(utilisateur);
            await unitOfWork.SauvegarderAsync();
        }
    }

    private static ClaimsPrincipal CreerClaimsPrincipal(Guid idUsager, string nomUsager, params Role[] roles)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.PrimarySid, idUsager.ToString()),
            new(ClaimTypes.Name, nomUsager), new(ClaimTypes.Expiration, DateTime.Now
                .AddHours(ExpirationEnHeures).ToString("O", CultureInfo.InvariantCulture))
        }.Union(roles.Select(role => new Claim(ClaimTypes.Role, role.ToString()))), "Basic"));
    }
}