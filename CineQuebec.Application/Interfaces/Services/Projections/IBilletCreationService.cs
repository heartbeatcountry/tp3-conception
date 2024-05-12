namespace CineQuebec.Application.Interfaces.Services.Projections;

public interface IBilletCreationService
{
    Task ReserverProjection(Guid idProjection);
    Task OffrirBilletGratuit(Guid idProjection, Guid idUtilisateur);
}