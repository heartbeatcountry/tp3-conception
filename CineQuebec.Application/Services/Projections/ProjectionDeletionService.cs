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

        unitOfWork.ProjectionRepository.Supprimer(projection);
        await unitOfWork.SauvegarderAsync();

        return true;
    }
}