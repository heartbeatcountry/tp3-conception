using CineQuebec.Application.Records.Abstract;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Records.Projections;

public record ProjectionDto(Guid Id, FilmDto? Film, SalleDto? Salle, DateTime DateHeure, bool EstAvantPremiere)
    : EntityDto(Id);

internal static class ProjectionExtensions
{
    internal static ProjectionDto VersDto(this IProjection projection, FilmDto? filmDto, SalleDto? salleDto)
    {
        return new ProjectionDto(projection.Id, filmDto, salleDto, projection.DateHeure.ToLocalTime(),
            projection.EstAvantPremiere);
    }
}