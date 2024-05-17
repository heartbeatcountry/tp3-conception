using CineQuebec.Application.Records.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Records.Projections;

public record ProjectionDto(Guid Id, SalleDto? Salle, DateTime DateHeure, bool EstAvantPremiere)
    : EntityDto(Id);

internal static class ProjectionExtensions
{
    internal static ProjectionDto VersDto(this IProjection projection, SalleDto? salleDto)
    {
        return new ProjectionDto(projection.Id, salleDto, projection.DateHeure.ToLocalTime(),
            projection.EstAvantPremiere);
    }
}