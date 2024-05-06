namespace CineQuebec.Application.Interfaces.Services.Films;

public interface IActeurCreationService
{
    Task<Guid> CreerActeur(string prenom, string nom);
}