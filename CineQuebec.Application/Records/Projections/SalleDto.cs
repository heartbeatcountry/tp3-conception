using CineQuebec.Application.Records.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Records.Projections;

public record class SalleDto(Guid Id, byte Numero, ushort NbSieges) : EntityDto(Id);

internal static class SalleExtensions
{
    internal static SalleDto VersDto(this ISalle salle)
    {
        return new SalleDto(salle.Id, salle.Numero, salle.NbSieges);
    }
}