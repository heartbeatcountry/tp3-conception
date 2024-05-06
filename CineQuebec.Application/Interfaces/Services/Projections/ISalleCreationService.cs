namespace CineQuebec.Application.Interfaces.Services.Projections;

public interface ISalleCreationService
{
    Task<Guid> CreerSalle(byte numero, ushort nbSieges);
}