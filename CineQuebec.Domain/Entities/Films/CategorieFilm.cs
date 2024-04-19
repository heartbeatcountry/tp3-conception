using System.Diagnostics.CodeAnalysis;

using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class CategorieFilm : Entite, IComparable<CategorieFilm>, ICategorieFilm
{
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
        if (string.IsNullOrWhiteSpace(nomAffichage))
        {
            throw new ArgumentException("Le nom d'affichage ne peut pas être vide.", nameof(nomAffichage));
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