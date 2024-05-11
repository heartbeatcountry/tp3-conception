using System.ComponentModel.DataAnnotations.Schema;

using CineQuebec.Domain.Exceptions.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Abstract;

namespace CineQuebec.Domain.Entities.Abstract;

public abstract class Personne : Entite, IComparable<Personne>, IPersonne
{
    public const byte LongueurMinNom = 1;
    public const byte LongueurMaxNom = 50;
    public const byte LongueurMinPrenom = 1;
    public const byte LongueurMaxPrenom = 50;

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
        nom = nom.Trim();

        if (nom.Length is < LongueurMinNom or > LongueurMaxNom)
        {
            throw new PersonneNomOutOfRangeException(
                $"Le nom doit contenir entre {LongueurMinNom} et {LongueurMaxNom} caractères.", nameof(nom));
        }

        Nom = nom.Trim();
    }

    public void SetPrenom(string prenom)
    {
        prenom = prenom.Trim();

        if (prenom.Length is < LongueurMinPrenom or > LongueurMaxPrenom)
        {
            throw new PersonnePrenomOutOfRangeException(
                $"Le prénom doit contenir entre {LongueurMinPrenom} et {LongueurMaxPrenom} caractères.",
                nameof(prenom));
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