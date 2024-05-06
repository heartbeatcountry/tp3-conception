using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Interfaces.Services.Preferences;

public interface IRealisateursFavorisQueryService
{
    Task<IEnumerable<RealisateurDto>> ObtenirRealisateursFavoris();
}