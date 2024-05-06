namespace CineQuebec.Application.Interfaces.Services.Films;

public interface ICategorieFilmCreationService
{
    Task<Guid> CreerCategorie(string nomAffichage);
}