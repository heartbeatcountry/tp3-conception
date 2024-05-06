using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Interfaces.Services.Preferences;

public interface IActeursFavorisQueryService
{
    Task<IEnumerable<ActeurDto>> ObtenirActeursFavoris();
}