using CineQuebec.Domain.Entities.Films;
using CineQuebec.Domain.Entities.Projections;

namespace CineQuebec.Application.Interfaces.Services;

public interface IProjectionCreationService
{
    Task<Guid> CreerProjection(Film film, Salle salle, DateTime dateHeure, bool estAvantPremiere);

}
