using System.Collections.Immutable;

using CineQuebec.Domain.Interfaces.Entities.Abstract;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IActeur : IPersonne
{
    ImmutableArray<Guid> JoueDansFilmsAvecId { get; }
    bool AjouterFilm(Guid idFilm);
    bool RetirerFilm(Guid idFilm);
}