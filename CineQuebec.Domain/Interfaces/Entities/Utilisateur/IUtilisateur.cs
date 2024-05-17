using CineQuebec.Domain.Entities.Utilisateurs;
using CineQuebec.Domain.Interfaces.Entities.Abstract;

namespace CineQuebec.Domain.Interfaces.Entities.Utilisateur;

public interface IUtilisateur : IPersonne
{
    string Courriel { get; }
    string HashMotDePasse { get; }
    IEnumerable<Role> Roles { get; }
    IEnumerable<Guid> CategoriesPrefereesParId { get; }
    IEnumerable<Guid> ActeursFavorisParId { get; }
    IEnumerable<Guid> RealisateursFavorisParId { get; }
    void AddActeursFavoris(IEnumerable<Guid> acteurs);
    void RemoveActeursFavoris(IEnumerable<Guid> acteurs);
    void AddBillets(IEnumerable<Guid> billets);
    void AddCategoriesPreferees(IEnumerable<Guid> categories);
    void RemoveCategoriesPreferees(IEnumerable<Guid> categories);
    void AddRealisateursFavoris(IEnumerable<Guid> realisateurs);
    void RemoveRealisateursFavoris(IEnumerable<Guid> realisateurs);
    void SetHashMotDePasse(string motDePasse);
}