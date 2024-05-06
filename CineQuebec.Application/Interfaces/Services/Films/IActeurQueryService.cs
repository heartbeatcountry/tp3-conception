using CineQuebec.Application.Records.Films;

namespace CineQuebec.Application.Interfaces.Services.Films;

public interface IActeurQueryService
{
    Task<IEnumerable<ActeurDto>> ObtenirTous();
}