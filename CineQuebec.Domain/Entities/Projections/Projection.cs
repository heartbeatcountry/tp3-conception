using CineQuebec.Domain.Entities.Abstract;

namespace CineQuebec.Domain.Entities.Projections;

public class Projection : Entite, IComparable<Projection>
{
	public Projection(Guid film, Guid salle, DateTime dateHeure, bool estAvantPremiere)
	{
		SetFilm(film);
		SetSalle(salle);
		SetDateHeure(dateHeure);
		SetEstAvantPremiere(estAvantPremiere);
	}

	public Guid Film { get; private set; }
	public Guid Salle { get; private set; }
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

	private void SetFilm(Guid idFilm)
	{
		if (idFilm == Guid.Empty)
		{
			throw new ArgumentNullException(nameof(idFilm), "Le guid du film ne peut pas être nul.");
		}

		Film = idFilm;
	}

	private void SetSalle(Guid idSalle)
	{
		if (idSalle == Guid.Empty)
		{
			throw new ArgumentNullException(nameof(idSalle), "Le guid de la salle ne peut pas être nul.");
		}

		Salle = idSalle;
	}
}