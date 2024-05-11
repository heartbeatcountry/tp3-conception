using System.Diagnostics.CodeAnalysis;

using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Exceptions.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class CategorieFilm : Entite, IComparable<CategorieFilm>, ICategorieFilm
{
    public const byte LongueurMinNomAffichage = 1;
    public const byte LongueurMaxNomAffichage = 50;

    public CategorieFilm(string nomAffichage)
    {
        SetNomAffichage(nomAffichage);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private CategorieFilm(Guid id, string nomAffichage) : this(nomAffichage)
    {
        // Constructeur avec identifiant pour Entity Framework Core
        SetId(id);
    }

    public string NomAffichage { get; private set; } = string.Empty;

    public void SetNomAffichage(string nomAffichage)
    {
        nomAffichage = nomAffichage.Trim();

        if (nomAffichage.Length is < LongueurMinNomAffichage or > LongueurMaxNomAffichage)
        {
            throw new NomAffichageOutOfRangeException(
                $"Le nom d'affichage doit contenir entre {LongueurMinNomAffichage} et {LongueurMaxNomAffichage} caractères.",
                nameof(nomAffichage));
        }

        NomAffichage = nomAffichage.Trim();
    }

    public override string ToString()
    {
        return NomAffichage;
    }

    public int CompareTo(CategorieFilm? other)
    {
        return ReferenceEquals(this, other) ? 0 :
            other is null ? 1 :
            string.Compare(NomAffichage, other.NomAffichage, StringComparison.Ordinal);
    }

    public new bool Equals(Entite? autre)
    {
        return base.Equals(autre) || (autre is CategorieFilm categorie &&
                                      string.Equals(NomAffichage, categorie.NomAffichage,
                                          StringComparison.OrdinalIgnoreCase));
    }
}