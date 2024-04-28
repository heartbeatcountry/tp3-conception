using System.ComponentModel.DataAnnotations.Schema;

using CineQuebec.Domain.Interfaces.Entities.Abstract;

namespace CineQuebec.Domain.Entities.Abstract;

public abstract class Personne : Entite, IComparable<Personne>, IPersonne
{
    protected Personne(string prenom, string nom)
    {
        SetPrenom(prenom);
        SetNom(nom);
    }

    public int CompareTo(Personne? other)
    {
        return ReferenceEquals(this, other) ? 0 :
            other is null ? 1 :
            string.Compare(NomComplet, other.NomComplet, StringComparison.OrdinalIgnoreCase);
    }

    public string Prenom { get; private set; } = string.Empty;
    public string Nom { get; private set; } = string.Empty;

    [NotMapped] public string NomComplet => $"{Prenom} {Nom}";

    public void SetNom(string nom)
    {
        if (string.IsNullOrWhiteSpace(nom))
        {
            throw new ArgumentException("Le nom ne peut pas être vide.", nameof(nom));
        }

        Nom = nom.Trim();
    }

    public void SetPrenom(string prenom)
    {
        if (string.IsNullOrWhiteSpace(prenom))
        {
            throw new ArgumentException("Le prénom ne peut pas être vide.", nameof(prenom));
        }

        Prenom = prenom.Trim();
    }

    public override string ToString()
    {
        return NomComplet;
    }

    public new bool Equals(Entite? autre)
    {
        return base.Equals(autre) || (autre is Personne personne &&
                                      string.Equals(NomComplet, personne.NomComplet,
                                          StringComparison.OrdinalIgnoreCase));
    }
}