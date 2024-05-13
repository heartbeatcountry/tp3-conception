using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Application.Records.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services.Projections;

public class SalleQueryService(IUnitOfWorkFactory unitOfWorkFactory) : ISalleQueryService
{
    public async Task<IEnumerable<SalleDto>> ObtenirToutes()
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IEnumerable<ISalle> salles = await unitOfWork.SalleRepository.ObtenirTousAsync();

        return salles.Select(s => s.VersDto()).OrderBy(salle => salle.Numero);
    }

    public async Task<ushort> ObtenirNbPlacesRestantes(Guid idProjection)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();

        IProjection projection = await unitOfWork.ProjectionRepository.ObtenirParIdAsync(idProjection) ??
                                 throw new KeyNotFoundException("Impossible de trouver la projection");
        ISalle salle = await unitOfWork.SalleRepository.ObtenirParIdAsync(projection.IdSalle) ??
                       throw new KeyNotFoundException("Impossible de trouver la salle");
        int nbBilletsVendus = await unitOfWork.BilletRepository.CompterAsync(b => b.IdProjection == idProjection);
        ushort nbPlacesRestantes = (ushort)Math.Max(0, salle.NbSieges - nbBilletsVendus);

        return nbPlacesRestantes;
    }
}