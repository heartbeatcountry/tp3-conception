using CineQuebec.Application.Records.Identity;

namespace CineQuebec.Application.Interfaces.Services.Fidelite;

public interface IUtilisateurFideliteQueryService
{
    Task<IEnumerable<UtilisateurDto>> ObtenirUtilisateursFideles(Guid idProjection);
}