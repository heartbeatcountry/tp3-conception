namespace CineQuebec.Application.Interfaces.Services.Identity;

public interface IUtilisateurCreationService
{
    Task<Guid> CreerUtilisateurAsync(string prenom, string nom, string courriel, string mdp);
}