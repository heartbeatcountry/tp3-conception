using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class CategorieFilm : Entite, IComparable<CategorieFilm>, ICategorieFilm
{
	public CategorieFilm(string nomAffichage)
	{
		SetNomAffichage(nomAffichage);
	}

	public string NomAffichage { get; private set; } = string.Empty;

	public int CompareTo(CategorieFilm? other)
	{
		if (ReferenceEquals(this, other))
		{
			return 0;
		}

		if (ReferenceEquals(null, other))
		{
			return 1;
		}

		return string.Compare(NomAffichage, other.NomAffichage, StringComparison.Ordinal);
	}

	public new bool Equals(Entite? autre)
	{
		return base.Equals(autre) || (autre is CategorieFilm categorie &&
		                              string.Equals(NomAffichage, categorie.NomAffichage,
			                              StringComparison.OrdinalIgnoreCase));
	}

	public void SetNomAffichage(string nomAffichage)
	{
		if (string.IsNullOrWhiteSpace(nomAffichage))
		{
			throw new ArgumentException("Le nom d'affichage ne peut pas être vide.", nameof(nomAffichage));
		}

		NomAffichage = nomAffichage.Trim();
	}

	public override string ToString()
	{
		return NomAffichage;
	}
}