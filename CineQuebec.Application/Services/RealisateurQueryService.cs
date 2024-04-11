using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class RealisateurQueryService(IUnitOfWorkFactory unitOfWorkFactory) : IRealisateurQueryService
{
    public async Task<IEnumerable<RealisateurDto>> ObtenirTous()
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IEnumerable<IRealisateur> realisateurs = await unitOfWork.RealisateurRepository.ObtenirTousAsync();

        return realisateurs.Select(realisateur => realisateur.VersDto())
            .OrderBy(realisateur => realisateur.Prenom);
    }
}