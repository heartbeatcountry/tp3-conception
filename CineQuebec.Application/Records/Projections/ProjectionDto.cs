using CineQuebec.Application.Records.Abstract;
using CineQuebec.Application.Records.Films;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Records.Projections;

public record class ProjectionDto(Guid Id, FilmDto? film, SalleDto? salle, DateTime DateHeure, bool EstAvantPremiere)
    : EntityDto(Id);

internal static class ProjectionExtensions
{
    internal static ProjectionDto VersDto(this IProjection projection, FilmDto? filmDto, SalleDto? salleDto)
    {
        return new ProjectionDto(projection.Id, filmDto, salleDto, projection.DateHeure, projection.EstAvantPremiere);
    }
}