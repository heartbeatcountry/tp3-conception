using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Projections;

namespace CineQuebec.Domain.Entities.Utilisateurs;

public class Utilisateur : Entite
{
	public static readonly byte MaxActeursFavoris = 5;
	public static readonly byte MaxRealisateursFavoris = 5;
	public static readonly byte MaxCategoriesPreferees = 3;
	private readonly HashSet<Acteur> _acteursFavoris = [];
	private readonly HashSet<Billet> _billets = [];
	private readonly HashSet<CategorieFilm> _categoriesPreferees = [];
	private readonly HashSet<Realisateur> _realisateursFavoris = [];
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
	public ImmutableArray<CategorieFilm> CategoriesPreferees => _categoriesPreferees.ToImmutableArray();
	public ImmutableArray<Acteur> ActeursFavoris => _acteursFavoris.ToImmutableArray();
	public ImmutableArray<Realisateur> RealisateursFavoris => _realisateursFavoris.ToImmutableArray();
	public ImmutableArray<Billet> Billets => _billets.ToImmutableArray();

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

	private void AddRoles(IEnumerable<Role> roles)
	{
		_roles.UnionWith(roles);
	}

	private void AddCategoriesPreferees(IEnumerable<CategorieFilm> categories)
	{
		var copie = _categoriesPreferees.ToHashSet();
		copie.UnionWith(categories);

		if (copie.Count > MaxCategoriesPreferees)
		{
			throw new ArgumentException(
				$"Impossible de compléter l'opération, puisque le nombre de catégories préférées ne peut pas dépasser {MaxCategoriesPreferees}.",
				nameof(categories));
		}

		_categoriesPreferees.UnionWith(copie);
	}

	private void AddActeursFavoris(IEnumerable<Acteur> acteurs)
	{
		var copie = _acteursFavoris.ToHashSet();
		copie.UnionWith(acteurs);

		if (copie.Count > MaxActeursFavoris)
		{
			throw new ArgumentException(
				$"Impossible de compléter l'opération, puisque le nombre d'acteurs favoris ne peut pas dépasser {MaxActeursFavoris}.",
				nameof(acteurs));
		}

		_acteursFavoris.UnionWith(copie);
	}

	private void AddRealisateursFavoris(IEnumerable<Realisateur> realisateurs)
	{
		var copie = _realisateursFavoris.ToHashSet();
		copie.UnionWith(realisateurs);

		if (copie.Count > MaxRealisateursFavoris)
		{
			throw new ArgumentException(
				$"Impossible de compléter l'opération, puisque le nombre de réalisateurs favoris ne peut pas dépasser {MaxRealisateursFavoris}.",
				nameof(realisateurs));
		}

		_realisateursFavoris.UnionWith(copie);
	}

	private void AddBillets(IEnumerable<Billet> billets)
	{
		_billets.UnionWith(billets);
	}
}