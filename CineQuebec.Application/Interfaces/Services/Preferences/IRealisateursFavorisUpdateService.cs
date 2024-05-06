namespace CineQuebec.Application.Interfaces.Services.Preferences;

public interface IRealisateursFavorisUpdateService
{
    Task AjouterRealisateurFavori(Guid idRealisateur);
    Task RetirerRealisateurFavori(Guid idRealisateur);
}