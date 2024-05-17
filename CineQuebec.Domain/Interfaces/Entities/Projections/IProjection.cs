using CineQuebec.Domain.Interfaces.Entities.Abstract;

namespace CineQuebec.Domain.Interfaces.Entities.Projections;

public interface IProjection : IEntite
{
    Guid IdFilm { get; }
    Guid IdSalle { get; }
    DateTime DateHeure { get; }
    bool EstAvantPremiere { get; }
}