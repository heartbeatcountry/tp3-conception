using CineQuebec.Application.Records.Projections;

namespace CineQuebec.Application.Interfaces.Services.Projections;

public interface IBilletQueryService
{
    Task<IEnumerable<BilletDto>> ObtenirTous();
}