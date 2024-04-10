namespace CineQuebec.Application.Interfaces.Services;

public interface IProjectionCreationService
{
    Task<Guid> CreerProjection(Guid pFilm, Guid pSalle, DateTime pDateHeure, bool pEstAvantPremiere);
}