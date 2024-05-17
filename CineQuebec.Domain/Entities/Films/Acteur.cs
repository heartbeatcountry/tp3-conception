using System.Collections.Immutable;

using CineQuebec.Domain.Entities.Abstract;
using CineQuebec.Domain.Exceptions.Entities.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Domain.Entities.Films;

public class Acteur(string prenom, string nom) : Personne(prenom, nom), IActeur
{
    private readonly HashSet<Guid> _joueDansFilmsAvecId = [];

    private Acteur(Guid id, string prenom, string nom) : this(prenom, nom)
    {
        // Constructeur avec identifiant pour Entity Framework Core
        SetId(id);
    }

    public ImmutableArray<Guid> JoueDansFilmsAvecId => [.. _joueDansFilmsAvecId];

    public bool AjouterFilm(Guid idFilm)
    {
        return idFilm == Guid.Empty
            ? throw new FilmGuidNullException("L'identifiant du film ne peut pas être vide.", nameof(idFilm))
            : _joueDansFilmsAvecId.Add(idFilm);
    }

    public bool RetirerFilm(Guid idFilm)
    {
        return _joueDansFilmsAvecId.Remove(idFilm);
    }
}