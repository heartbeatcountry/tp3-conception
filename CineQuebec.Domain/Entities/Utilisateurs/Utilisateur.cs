using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;

namespace CineQuebec.Domain.Entities.Utilisateurs;

public class Utilisateur : Entite
{
	public const byte MaxActeursFavoris = 5;
	public const byte MaxRealisateursFavoris = 5;
	public const byte MaxCategoriesPreferees = 3;
	private readonly HashSet<Guid> _acteursFavorisParId = [];
	private readonly HashSet<Guid> _billetsParId = [];
	private readonly HashSet<Guid> _categoriesPrefereesParId = [];
	private readonly HashSet<Guid> _realisateursFavorisParId = [];
	private readonly HashSet<Role> _roles = [];

	public Utilisateur(string courriel, string motDePasse, IEnumerable<Role> roles)
	{
		SetCourriel(courriel);
		SetMotDePasse(motDePasse);
		AddRoles(roles);
	}

	public string Courriel { get; private set; } = string.Empty;
	public string HashMotDePasse { get; private set; } = string.Empty;
	public ImmutableArray<Role> Roles => _roles.ToImmutableArray();
	public ImmutableArray<Guid> CategoriesPrefereesParId => _categoriesPrefereesParId.ToImmutableArray();
	public ImmutableArray<Guid> ActeursFavorisParId => _acteursFavorisParId.ToImmutableArray();
	public ImmutableArray<Guid> RealisateursFavorisParId => _realisateursFavorisParId.ToImmutableArray();
	public ImmutableArray<Guid> BilletsParId => _billetsParId.ToImmutableArray();

	private void AddActeursFavoris(IEnumerable<Guid> acteurs)
	{
		var copie = _acteursFavorisParId.ToHashSet();
		copie.UnionWith(acteurs);

		if (copie.Count > MaxActeursFavoris)
		{
			throw new ArgumentException(
				$"Impossible de compléter l'opération, puisque le nombre d'acteurs favoris ne peut pas dépasser {MaxActeursFavoris}.",
				nameof(acteurs));
		}

		_acteursFavorisParId.UnionWith(copie);
	}

	private void AddBillets(IEnumerable<Guid> billets)
	{
		_billetsParId.UnionWith(billets);
	}

	private void AddCategoriesPreferees(IEnumerable<Guid> categories)
	{
		var copie = _categoriesPrefereesParId.ToHashSet();
		copie.UnionWith(categories);

		if (copie.Count > MaxCategoriesPreferees)
		{
			throw new ArgumentException(
				$"Impossible de compléter l'opération, puisque le nombre de catégories préférées ne peut pas dépasser {MaxCategoriesPreferees}.",
				nameof(categories));
		}

		_categoriesPrefereesParId.UnionWith(copie);
	}

	private void AddRealisateursFavoris(IEnumerable<Guid> realisateurs)
	{
		var copie = _realisateursFavorisParId.ToHashSet();
		copie.UnionWith(realisateurs);

		if (copie.Count > MaxRealisateursFavoris)
		{
			throw new ArgumentException(
				$"Impossible de compléter l'opération, puisque le nombre de réalisateurs favoris ne peut pas dépasser {MaxRealisateursFavoris}.",
				nameof(realisateurs));
		}

		_realisateursFavorisParId.UnionWith(copie);
	}

	private void AddRoles(IEnumerable<Role> roles)
	{
		_roles.UnionWith(roles);
	}

	public new bool Equals(Entite? autre)
	{
		return base.Equals(autre) || (autre is Utilisateur utilisateur &&
		                              string.Equals(Courriel, utilisateur.Courriel,
			                              StringComparison.OrdinalIgnoreCase));
	}

	private void SetCourriel(string courriel)
	{
		if (string.IsNullOrWhiteSpace(courriel))
		{
			throw new ArgumentException("Le courriel ne peut pas être vide.", nameof(courriel));
		}

		Courriel = courriel.Trim().ToLowerInvariant();
	}

	private void SetMotDePasse(string motDePasse)
	{
		if (string.IsNullOrWhiteSpace(motDePasse))
		{
			throw new ArgumentException("Le mot de passe ne peut pas être vide.", nameof(motDePasse));
		}

		HashMotDePasse = motDePasse.Trim();
	}
}