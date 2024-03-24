using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;

namespace CineQuebec.Domain.Entities.Films;

public class Film : Entite, IComparable<Film>
{
	private readonly HashSet<Acteur> _acteurs = [];
	private readonly HashSet<Realisateur> _realisateurs = [];

	public Film(string titre, string description, CategorieFilm categorie, DateTime dateSortieInternationale,
		IEnumerable<Acteur> acteurs, IEnumerable<Realisateur> realisateurs, ushort duree)
	{
		SetTitre(titre);
		SetDescription(description);
		SetCategorie(categorie);
		SetDateSortieInternationale(dateSortieInternationale);
		AddActeurs(acteurs);
		AddRealisateurs(realisateurs);
		SetDuree(duree);
	}

	public string Titre { get; private set; } = string.Empty;
	public string Description { get; private set; } = string.Empty;
	public CategorieFilm Categorie { get; private set; } = null!;
	public DateTime DateSortieInternationale { get; private set; } = DateTime.MinValue;
	public ImmutableArray<Acteur> Acteurs => _acteurs.ToImmutableArray();
	public ImmutableArray<Realisateur> Realisateurs => _realisateurs.ToImmutableArray();
	public ushort Duree { get; private set; }

	public int CompareTo(Film? other)
	{
		if (ReferenceEquals(this, other))
		{
			return 0;
		}

		if (ReferenceEquals(null, other))
		{
			return 1;
		}

		var dateComparison = DateSortieInternationale.CompareTo(other.DateSortieInternationale);
		return dateComparison != 0 ? dateComparison : string.Compare(Titre, other.Titre, StringComparison.Ordinal);
	}

	public new bool Equals(Entite? autre)
	{
		return base.Equals(autre) || (autre is Film film &&
		                              string.Equals(Titre, film.Titre,
			                              StringComparison.OrdinalIgnoreCase) && DateSortieInternationale.Year ==
		                              film.DateSortieInternationale.Year && Duree == film.Duree);
	}

	private void SetTitre(string titre)
	{
		if (string.IsNullOrWhiteSpace(titre))
		{
			throw new ArgumentException("Le titre ne peut pas être vide.", nameof(titre));
		}

		Titre = titre.Trim();
	}

	private void SetDescription(string description)
	{
		if (string.IsNullOrWhiteSpace(description))
		{
			throw new ArgumentException("La description ne peut pas être vide.", nameof(description));
		}

		Description = description.Trim();
	}

	private void SetCategorie(CategorieFilm categorie)
	{
		if (categorie == null)
		{
			throw new ArgumentNullException(nameof(categorie), "La catégorie ne peut pas être nulle.");
		}

		Categorie = categorie;
	}

	private void SetDateSortieInternationale(DateTime dateSortieInternationale)
	{
		if (dateSortieInternationale == DateTime.MinValue)
		{
			throw new ArgumentNullException(nameof(dateSortieInternationale),
				"La date de sortie internationale ne peut pas être nulle.");
		}

		DateSortieInternationale = dateSortieInternationale;
	}

	private void AddActeurs(IEnumerable<Acteur> acteurs)
	{
		_acteurs.UnionWith(acteurs);
	}

	private void AddRealisateurs(IEnumerable<Realisateur> realisateurs)
	{
		_realisateurs.UnionWith(realisateurs);
	}

	private void SetDuree(ushort duree)
	{
		if (duree == 0)
		{
			throw new ArgumentNullException(nameof(duree), "Le film doit durer plus de 0 minutes.");
		}

		Duree = duree;
	}

	public override string ToString()
	{
		return $"{Titre} ({DateSortieInternationale.Year})";
	}
}