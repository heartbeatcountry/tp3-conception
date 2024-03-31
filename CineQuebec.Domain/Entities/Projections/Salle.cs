using CineQuebec.Domain.Entities.Abstract;

namespace CineQuebec.Domain.Entities.Projections;

public class Salle : Entite, IComparable<Salle>
{
	public Salle(byte numero, ushort nbSieges)
	{
		SetNumero(numero);
		SetNbSieges(nbSieges);
	}

	public byte Numero { get; private set; }
	public ushort NbSieges { get; private set; }

	public int CompareTo(Salle? other)
	{
		if (ReferenceEquals(this, other))
		{
			return 0;
		}

		if (ReferenceEquals(null, other))
		{
			return 1;
		}

		return Numero.CompareTo(other.Numero);
	}

	public new bool Equals(Entite? autre)
	{
		return base.Equals(autre) || (autre is Salle salle && Numero == salle.Numero);
	}

	private void SetNbSieges(ushort nbSieges)
	{
		if (nbSieges == 0)
		{
			throw new ArgumentException("Le nombre de sièges ne peut pas être zéro.", nameof(nbSieges));
		}

		NbSieges = nbSieges;
	}

	private void SetNumero(byte numero)
	{
		if (numero == 0)
		{
			throw new ArgumentException("Le numéro de salle ne peut pas être zéro.", nameof(numero));
		}

		Numero = numero;
	}
}