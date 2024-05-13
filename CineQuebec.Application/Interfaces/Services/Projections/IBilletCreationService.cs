namespace CineQuebec.Application.Interfaces.Services.Projections;

public interface IBilletCreationService
{
    Task ReserverProjection(Guid idProjection, ushort nbBillets = 1);
    Task OffrirBilletGratuit(Guid idProjection, Guid idUtilisateur);
}