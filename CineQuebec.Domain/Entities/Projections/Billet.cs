using CineQuebec.Domain.Entities.Abstract;

namespace CineQuebec.Domain.Entities.Projections;

public class Billet : Entite, IComparable<Billet>
{
	public Billet(Guid projection, ushort numSiege)
	{
		SetProjection(projection);
		SetNumSiege(numSiege);
	}

	public Guid Projection { get; private set; }
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

	private void SetNumSiege(ushort numSiege)
	{
		if (numSiege == 0)
		{
			throw new ArgumentException("Le numéro de siège ne peut pas être zéro.", nameof(numSiege));
		}

		NumSiege = numSiege;
	}

	private void SetProjection(Guid projection)
	{
		Projection = projection;
	}
}