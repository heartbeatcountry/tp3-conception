using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Entities.Films;

namespace CineQuebec.Domain.Entities.Projections;

public class Projection : Entite, IComparable<Projection>
{
	public Projection(Film film, Salle salle, DateTime dateHeure, bool estAvantPremiere)
	{
		SetFilm(film);
		SetSalle(salle);
		SetDateHeure(dateHeure);
		SetEstAvantPremiere(estAvantPremiere);
	}

	public Film Film { get; private set; } = null!;
	public Salle Salle { get; private set; } = null!;
	public DateTime DateHeure { get; private set; } = DateTime.MinValue;
	public bool EstAvantPremiere { get; private set; }

	public int CompareTo(Projection? other)
	{
		if (ReferenceEquals(this, other))
		{
			return 0;
		}

		if (ReferenceEquals(null, other))
		{
			return 1;
		}

		return DateHeure.CompareTo(other.DateHeure);
	}

	public new bool Equals(Entite? autre)
	{
		return base.Equals(autre) || (autre is Projection projection && Salle.Equals(projection.Salle) &&
		                              DateHeure == projection.DateHeure);
	}

	private void SetFilm(Film film)
	{
		if (film == null)
		{
			throw new ArgumentNullException(nameof(film), "Le film ne peut pas être nul.");
		}

		Film = film;
	}

	private void SetSalle(Salle salle)
	{
		if (salle == null)
		{
			throw new ArgumentNullException(nameof(salle), "La salle ne peut pas être nulle.");
		}

		Salle = salle;
	}

	private void SetDateHeure(DateTime dateHeure)
	{
		if (dateHeure == DateTime.MinValue)
		{
			throw new ArgumentNullException(nameof(dateHeure), "La date et l'heure ne peuvent pas être nulles.");
		}

		DateHeure = dateHeure;
	}

	private void SetEstAvantPremiere(bool estAvantPremiere)
	{
		EstAvantPremiere = estAvantPremiere;
	}
}