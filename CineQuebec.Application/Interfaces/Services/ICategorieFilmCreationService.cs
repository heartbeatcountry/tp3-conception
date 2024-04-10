namespace CineQuebec.Application.Interfaces.Services;

public interface ICategorieFilmCreationService
{
    Task<Guid> CreerCategorie(string nomAffichage);
}