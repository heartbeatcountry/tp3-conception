using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Interfaces.Services;

public interface IRealisateurQueryService
{
    Task<IEnumerable<RealisateurDto>> ObtenirTous();
}