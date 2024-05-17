using CineQuebec.Application.Interfaces.DbContext;
using CineQuebec.Application.Interfaces.Services.Projections;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Services.Projections;

public class ProjectionDeletionService(IUnitOfWorkFactory unitOfWorkFactory) : IProjectionDeletionService
{
    public async Task<bool> SupprimerProjection(Guid id)
    {
        using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        IProjection? projection = await unitOfWork.ProjectionRepository.ObtenirParIdAsync(id);

        if (projection is null)
        {
            return false;
        }

        await SupprimerBillets(unitOfWork, id);
        unitOfWork.ProjectionRepository.Supprimer(projection);
        await unitOfWork.SauvegarderAsync();

        return true;
    }

    private static async Task SupprimerBillets(IUnitOfWork unitOfWork, Guid idProjection)
    {
        IEnumerable<IBillet> billets =
            await unitOfWork.BilletRepository.ObtenirTousAsync(b => b.IdProjection == idProjection);

        foreach (IBillet billet in billets)
        {
            unitOfWork.BilletRepository.Supprimer(billet);
        }
    }
}