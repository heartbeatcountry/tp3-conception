namespace CineQuebec.Application.Interfaces.Services;

public interface IActeurCreationService
{
    Task<Guid> CreerActeur(string prenom, string nom);
}