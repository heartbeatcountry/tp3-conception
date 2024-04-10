using CineQuebec.Application.Records.Projections;

namespace CineQuebec.Application.Interfaces.Services;

public interface IProjectionQueryService
{
    Task<IEnumerable<ProjectionDto>> ObtenirProjectionsAVenirPourFilm(Guid idFilm);
}