namespace CineQuebec.Application.Interfaces.Services.Preferences;

public interface IActeursFavorisUpdateService
{
    Task AjouterActeurFavori(Guid idActeur);
    Task RetirerActeurFavori(Guid idActeur);
}