using CineQuebec.Application.Records.Projections;

namespace CineQuebec.Application.Interfaces.Services;

public interface ISalleQueryService
{
    Task<IEnumerable<SalleDto>> ObtenirToutes();
}