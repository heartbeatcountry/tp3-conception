using CineQuebec.Domain.Interfaces.Entities;

namespace CineQuebec.Domain.Entities.Abstract;

public abstract class Entite : IEquatable<Entite>, IEntite
{
	public Guid Id { get; private set; } = Guid.Empty;

	public bool Equals(Entite? autre)
	{
		if (ReferenceEquals(null, autre))
		{
			return false;
		}

		if (ReferenceEquals(this, autre))
		{
			return true;
		}

		return Id.Equals(autre.Id);
	}

	public void SetId(Guid id)
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentNullException(nameof(id), "L'identifiant ne peut pas être nul.");
		}

		Id = id;
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj))
		{
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (obj.GetType() != GetType())
		{
			return false;
		}

		return Equals((Entite)obj);
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}

	public static bool operator ==(Entite? gauche, Entite? droite)
	{
		return Equals(gauche, droite);
	}

	public static bool operator !=(Entite? gauche, Entite? droite)
	{
		return !Equals(gauche, droite);
	}

	public override string ToString()
	{
		return $"{GetType().Name} {Id}";
	}
}