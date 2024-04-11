using System.Collections.Immutable;

namespace CineQuebec.Domain.Interfaces.Entities.Films;

public interface IRealisateur : IPersonne
{
    ImmutableArray<Guid> RealiseFilmsAvecId { get; }
    bool AjouterFilm(Guid idFilm);
    bool RetirerFilm(Guid idFilm);
}