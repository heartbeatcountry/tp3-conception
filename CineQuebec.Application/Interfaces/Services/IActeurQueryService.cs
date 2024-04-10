using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Interfaces.Services;

public interface IActeurQueryService
{
    Task<IEnumerable<ActeurDto>> ObtenirTous();
}