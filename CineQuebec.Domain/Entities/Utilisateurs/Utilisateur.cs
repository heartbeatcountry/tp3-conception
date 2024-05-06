using System.Diagnostics.CodeAnalysis;

using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Domain.Entities.Utilisateurs;

public class Utilisateur : Personne, IUtilisateur
{
    public const byte MaxActeursFavoris = 5;
    public const byte MaxRealisateursFavoris = 5;
    public const byte MaxCategoriesPreferees = 3;
    private readonly HashSet<Guid> _acteursFavorisParId = [];
    private readonly HashSet<Guid> _billetsParId = [];
    private readonly HashSet<Guid> _categoriesPrefereesParId = [];
    private readonly HashSet<Guid> _realisateursFavorisParId = [];
    private readonly HashSet<Role> _roles = [];

    public Utilisateur(string prenom, string nom, string courriel, string hashMotDePasse, IEnumerable<Role> roles) :
        base(prenom, nom)
    {
        SetPrenom(prenom);
        SetNom(nom);
        SetCourriel(courriel);
        SetHashMotDePasse(hashMotDePasse);
        AddRoles(roles);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Utilisateur(Guid id, string prenom, string nom, string courriel, string hashMotDePasse,
        IEnumerable<Role> roles,
        IEnumerable<Guid> categoriesPrefereesParId, IEnumerable<Guid> acteursFavorisParId,
        IEnumerable<Guid> realisateursFavorisParId, IEnumerable<Guid> billetsParId) : this(prenom, nom, courriel,
        hashMotDePasse, roles)
    {
        // Constructeur avec identifiant pour Entity Framework Core
        SetId(id);
        AddCategoriesPreferees(categoriesPrefereesParId);
        AddActeursFavoris(acteursFavorisParId);
        AddRealisateursFavoris(realisateursFavorisParId);
        AddBillets(billetsParId);
    }

    public string Courriel { get; private set; } = string.Empty;
    public string HashMotDePasse { get; private set; } = string.Empty;
    public IEnumerable<Role> Roles { get => [.. _roles]; private set => SetRoles(value); }

    public IEnumerable<Guid> CategoriesPrefereesParId
    {
        get => [.. _categoriesPrefereesParId];
        private set => SetCategoriesPrefereesParId(value);
    }

    public IEnumerable<Guid> ActeursFavorisParId
    {
        get => [.. _acteursFavorisParId];
        private set => SetActeursFavorisParId(value);
    }

    public IEnumerable<Guid> RealisateursFavorisParId
    {
        get => [.. _realisateursFavorisParId];
        private set => SetRealisateursFavorisParId(value);
    }

    public IEnumerable<Guid> BilletsParId
    {
        get => [.. _billetsParId];
        private set => SetBilletsParId(value);
    }

    public void AddActeursFavoris(IEnumerable<Guid> acteurs)
    {
        HashSet<Guid> copie = _acteursFavorisParId.Union(acteurs).ToHashSet();

        if (copie.Count > MaxActeursFavoris)
        {
            throw new ArgumentException(
                $"Impossible de compléter l'opération, puisque le nombre d'acteurs favoris ne doit pas dépasser {MaxActeursFavoris}.",
                nameof(acteurs));
        }

        SetActeursFavorisParId(copie);
    }

    public void RemoveActeursFavoris(IEnumerable<Guid> acteurs)
    {
        _acteursFavorisParId.RemoveWhere(acteurs.Contains);
    }

    public void AddBillets(IEnumerable<Guid> billets)
    {
        SetBilletsParId(_billetsParId.Union(billets));
    }

    public void AddCategoriesPreferees(IEnumerable<Guid> categories)
    {
        HashSet<Guid> copie = _categoriesPrefereesParId.Union(categories).ToHashSet();

        if (copie.Count > MaxCategoriesPreferees)
        {
            throw new ArgumentException(
                $"Impossible de compléter l'opération, puisque le nombre de catégories préférées ne doit pas dépasser {MaxCategoriesPreferees}.",
                nameof(categories));
        }

        SetCategoriesPrefereesParId(copie);
    }

    public void RemoveCategoriesPreferees(IEnumerable<Guid> categories)
    {
        _categoriesPrefereesParId.RemoveWhere(categories.Contains);
    }

    public void AddRealisateursFavoris(IEnumerable<Guid> realisateurs)
    {
        HashSet<Guid> copie = _realisateursFavorisParId.Union(realisateurs).ToHashSet();

        if (copie.Count > MaxRealisateursFavoris)
        {
            throw new ArgumentException(
                $"Impossible de compléter l'opération, puisque le nombre de réalisateurs favoris ne doit pas dépasser {MaxRealisateursFavoris}.",
                nameof(realisateurs));
        }

        SetRealisateursFavorisParId(copie);
    }

    public void RemoveRealisateursFavoris(IEnumerable<Guid> realisateurs)
    {
        _realisateursFavorisParId.RemoveWhere(realisateurs.Contains);
    }

    public void AddRoles(IEnumerable<Role> roles)
    {
        SetRoles(_roles.Union(roles).ToHashSet());
    }

    public void SetHashMotDePasse(string motDePasse)
    {
        if (string.IsNullOrWhiteSpace(motDePasse))
        {
            throw new ArgumentException("Le mot de passe ne doit pas être vide.", nameof(motDePasse));
        }

        HashMotDePasse = motDePasse.Trim();
    }

    public new bool Equals(Entite? autre)
    {
        return autre is not null && (ReferenceEquals(this, autre) || Id.Equals(autre.Id) ||
                                     (autre is Utilisateur utilisateur && string.Equals(Courriel, utilisateur.Courriel,
                                         StringComparison.OrdinalIgnoreCase)));
    }

    private void SetActeursFavorisParId(IEnumerable<Guid> acteurs)
    {
        _acteursFavorisParId.Clear();
        _acteursFavorisParId.UnionWith(acteurs);
    }

    private void SetBilletsParId(IEnumerable<Guid> billets)
    {
        _billetsParId.Clear();
        _billetsParId.UnionWith(billets);
    }

    private void SetCategoriesPrefereesParId(IEnumerable<Guid> categories)
    {
        _categoriesPrefereesParId.Clear();
        _categoriesPrefereesParId.UnionWith(categories);
    }

    private void SetCourriel(string courriel)
    {
        if (string.IsNullOrWhiteSpace(courriel))
        {
            throw new ArgumentException("Le courriel ne doit pas être vide.", nameof(courriel));
        }

        Courriel = courriel.Trim().ToLowerInvariant();
    }

    private void SetRealisateursFavorisParId(IEnumerable<Guid> realisateurs)
    {
        _realisateursFavorisParId.Clear();
        _realisateursFavorisParId.UnionWith(realisateurs);
    }

    private void SetRoles(IEnumerable<Role> roles)
    {
        _roles.Clear();
        _roles.UnionWith([..roles, Role.Utilisateur]);
    }
}