namespace CineQuebec.Application.Interfaces.Services;

public interface IProjectionDeletionService
{
    Task<bool> SupprimerProjection(Guid id);
}