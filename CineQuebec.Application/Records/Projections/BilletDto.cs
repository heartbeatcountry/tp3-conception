using CineQuebec.Application.Records.Abstract;
using CineQuebec.Application.Records.Identity;
using CineQuebec.Domain.Interfaces.Entities.Projections;

namespace CineQuebec.Application.Records.Projections;

public record BilletDto(
    Guid Id,
    ProjectionDto Projection,
    UtilisateurDto? Utilisateur
) : EntityDto(Id);

internal static class BilletExtensions
{
    internal static BilletDto VersDto(this IBillet pBillet, ProjectionDto pProjection, UtilisateurDto? pUtilisateur)
    {
        return new BilletDto(pBillet.Id, pProjection, pUtilisateur);
    }
}