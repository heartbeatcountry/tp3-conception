using System.Collections.Immutable;
using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class Film : Entite, IComparable<Film>, IFilm
{
	private readonly HashSet<IActeur> _acteurs = [];
	private readonly HashSet<IRealisateur> _realisateurs = [];

	public Film(string titre, string description, ICategorieFilm categorie, DateOnly dateSortieInternationale,
		IEnumerable<IActeur> acteurs, IEnumerable<IRealisateur> realisateurs, ushort duree)
	{
		SetTitre(titre);
		SetDescription(description);
		SetCategorie(categorie);
		SetDateSortieInternationale(dateSortieInternationale);
		AddActeurs(acteurs);
		AddRealisateurs(realisateurs);
		SetDureeEnMinutes(duree);
	}

	public string Titre { get; private set; } = string.Empty;
	public string Description { get; private set; } = string.Empty;
	public ICategorieFilm Categorie { get; private set; } = null!;
	public DateOnly DateSortieInternationale { get; private set; } = DateOnly.MinValue;
	public ImmutableArray<IActeur> Acteurs => _acteurs.ToImmutableArray();
	public ImmutableArray<IRealisateur> Realisateurs => _realisateurs.ToImmutableArray();
	public ushort DureeEnMinutes { get; private set; }

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
		                              film.DateSortieInternationale.Year && DureeEnMinutes == film.DureeEnMinutes);
	}

	public void SetTitre(string titre)
	{
		if (string.IsNullOrWhiteSpace(titre))
		{
			throw new ArgumentException("Le titre ne peut pas être vide.", nameof(titre));
		}

		Titre = titre.Trim();
	}

	public void SetDescription(string description)
	{
		if (string.IsNullOrWhiteSpace(description))
		{
			throw new ArgumentException("La description ne peut pas être vide.", nameof(description));
		}

		Description = description.Trim();
	}

	public void SetCategorie(ICategorieFilm categorie)
	{
		Categorie = categorie ?? throw new ArgumentNullException(nameof(categorie), "La catégorie ne peut pas être nulle.");
	}

	public void SetDateSortieInternationale(DateOnly dateSortieInternationale)
	{
		if (dateSortieInternationale == DateOnly.MinValue)
		{
			throw new ArgumentOutOfRangeException(nameof(dateSortieInternationale),
				$"La date de sortie internationale doit être supérieure à {DateOnly.MinValue}.");
		}

		DateSortieInternationale = dateSortieInternationale;
	}

	public void AddActeurs(IEnumerable<IActeur> acteurs)
	{
		_acteurs.UnionWith(acteurs);
	}

	public void AddRealisateurs(IEnumerable<IRealisateur> realisateurs)
	{
		_realisateurs.UnionWith(realisateurs);
	}

	public void SetDureeEnMinutes(ushort duree)
	{
		if (duree == 0)
		{
			throw new ArgumentOutOfRangeException(nameof(duree), "Le film doit durer plus de 0 minutes.");
		}

		DureeEnMinutes = duree;
	}

	public override string ToString()
	{
		return $"{Titre} ({DateSortieInternationale.Year})";
	}
}