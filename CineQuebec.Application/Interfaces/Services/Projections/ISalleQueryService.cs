using CineQuebec.Application.Records.Projections;

namespace CineQuebec.Application.Interfaces.Services.Projections;

public interface ISalleQueryService
{
    Task<IEnumerable<SalleDto>> ObtenirToutes();
    Task<ushort> ObtenirNbPlacesRestantes(Guid idProjection);
}