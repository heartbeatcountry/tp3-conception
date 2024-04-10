using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Services;

public class ActeurQueryService(IUnitOfWorkFactory unitOfWorkFactory) : IActeurQueryService
{
    public async Task<IEnumerable<ActeurDto>> ObtenirTous()
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IEnumerable<IActeur> acteurs = await unitOfWork.ActeurRepository.ObtenirTousAsync();

        return acteurs.Select(acteur => acteur.VersDto());
    }
}