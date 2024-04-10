namespace CineQuebec.Application.Interfaces.Services;

public interface IRealisateurCreationService
{
    Task<Guid> CreerRealisateur(string nom, string prenom);
}