namespace CineQuebec.Application.Interfaces.Services;

public interface ISalleCreationService
{
    Task<Guid> CreerSalle(byte numero, ushort nbSieges);
}