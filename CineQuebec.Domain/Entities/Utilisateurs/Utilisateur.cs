using System.Collections.Immutable;
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
    private Utilisateur(Guid id, string prenom, string nom, string courriel, string hashMotDePasse, IEnumerable<Role> roles,
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
    public IEnumerable<Role> Roles => _roles.ToImmutableArray();
    public IEnumerable<Guid> CategoriesPrefereesParId => _categoriesPrefereesParId.ToImmutableArray();
    public IEnumerable<Guid> ActeursFavorisParId => _acteursFavorisParId.ToImmutableArray();
    public IEnumerable<Guid> RealisateursFavorisParId => _realisateursFavorisParId.ToImmutableArray();
    public IEnumerable<Guid> BilletsParId => _billetsParId.ToImmutableArray();

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

    public void AddRoles(IEnumerable<Role> roles)
    {
        SetRoles(_roles.Union(roles).ToHashSet());
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

    public void SetHashMotDePasse(string motDePasse)
    {
        if (string.IsNullOrWhiteSpace(motDePasse))
        {
            throw new ArgumentException("Le mot de passe ne doit pas être vide.", nameof(motDePasse));
        }

        HashMotDePasse = motDePasse.Trim();
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