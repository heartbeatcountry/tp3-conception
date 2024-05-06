namespace CineQuebec.Application.Interfaces.Services.Projections;

public interface IProjectionCreationService
{
    Task<Guid> CreerProjection(Guid pFilm, Guid pSalle, DateTime pDateHeure, bool pEstAvantPremiere);
}