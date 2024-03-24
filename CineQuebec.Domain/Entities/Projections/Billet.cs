using CineQuebec.Domain.Entities.Abstract;

namespace CineQuebec.Domain.Entities.Projections;

public class Billet : Entite, IComparable<Billet>
{
	public Billet(Projection projection, ushort numSiege)
	{
		SetProjection(projection);
		SetNumSiege(numSiege);
	}

	public Projection Projection { get; private set; } = null!;
	public ushort NumSiege { get; private set; }

	public int CompareTo(Billet? other)
	{
		if (ReferenceEquals(this, other))
		{
			return 0;
		}

		if (ReferenceEquals(null, other))
		{
			return 1;
		}

		return Projection.CompareTo(other.Projection);
	}

	public new bool Equals(Entite? autre)
	{
		return base.Equals(autre) || (autre is Billet billet && Projection.Equals(billet.Projection) &&
		                              NumSiege == billet.NumSiege);
	}

	private void SetProjection(Projection projection)
	{
		Projection = projection;
	}

	private void SetNumSiege(ushort numSiege)
	{
		if (numSiege == 0)
		{
			throw new ArgumentException("Le numéro de siège ne peut pas être zéro.", nameof(numSiege));
		}

		if (numSiege > Projection.Salle.NbSieges)
		{
			throw new ArgumentException("Le numéro de siège ne peut pas être plus grand que la capacité de la salle.",
				nameof(numSiege));
		}

		NumSiege = numSiege;
	}
}