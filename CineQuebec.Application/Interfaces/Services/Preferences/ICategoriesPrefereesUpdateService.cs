namespace CineQuebec.Application.Interfaces.Services.Preferences;

public interface ICategoriesPrefereesUpdateService
{
    Task AjouterCategoriePreferee(Guid idCategorie);
    Task RetirerCategoriePreferee(Guid idCategorie);
}