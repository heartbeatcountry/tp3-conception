using CineQuebec.Application.Records.Projections;

namespace CineQuebec.Application.Interfaces.Services;

internal interface ISalleQueryService
{
    Task<IEnumerable<SalleDto>> ObtenirToutes();
}