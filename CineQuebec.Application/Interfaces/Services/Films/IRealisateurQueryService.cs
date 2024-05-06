using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Interfaces.Services.Films;

public interface IRealisateurQueryService
{
    Task<IEnumerable<RealisateurDto>> ObtenirTous();
}