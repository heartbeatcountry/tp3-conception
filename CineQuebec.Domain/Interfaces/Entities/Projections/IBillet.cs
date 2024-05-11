using CineQuebec.Domain.Interfaces.Entities.Abstract;

namespace CineQuebec.Domain.Interfaces.Entities.Projections;

public interface IBillet : IEntite
{
    Guid IdProjection { get; }
    Guid IdUtilisateur { get; }
}