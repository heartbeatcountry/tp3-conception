using CineQuebec.Application.Records.Projections;

namespace CineQuebec.Application.Interfaces.Services.Projections;

public interface IProjectionQueryService
{
    Task<IEnumerable<ProjectionDto>> ObtenirProjectionsAVenirPourFilm(Guid idFilm);
}