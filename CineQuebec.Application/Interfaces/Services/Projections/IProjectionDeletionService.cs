namespace CineQuebec.Application.Interfaces.Services.Projections;

public interface IProjectionDeletionService
{
    Task<bool> SupprimerProjection(Guid id);
}