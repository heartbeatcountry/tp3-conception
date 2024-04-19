using CineQuebec.Domain.Interfaces.Entities;

namespace CineQuebec.Domain.Entities.Abstract;

public abstract class Entite : IEquatable<Entite>, IEntite
{
    public Guid Id { get; private set; } = Guid.Empty;

    public void SetId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id), "L'identifiant ne peut pas être nul.");
        }

        Id = id;
    }

    public override string ToString()
    {
        return $"{GetType().Name} {Id}";
    }

    public bool Equals(Entite? autre)
    {
        return autre is not null && (ReferenceEquals(this, autre) || Id.Equals(autre.Id));
    }

    public override bool Equals(object? obj)
    {
        return obj is not null && (ReferenceEquals(this, obj) || (obj.GetType() == GetType() && Equals((Entite)obj)));
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
}