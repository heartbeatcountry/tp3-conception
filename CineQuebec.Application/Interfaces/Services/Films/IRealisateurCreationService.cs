namespace CineQuebec.Application.Interfaces.Services.Films;

public interface IRealisateurCreationService
{
    Task<Guid> CreerRealisateur(string nom, string prenom);
}