using CineQuebec.Domain.Interfaces.Entities.Abstract;

namespace CineQuebec.Domain.Interfaces.Entities.Projections;

public interface ISalle : IEntite
{
    byte Numero { get; }
    ushort NbSieges { get; }
    void SetNbSieges(ushort nbSieges);
    void SetNumero(byte numero);
}