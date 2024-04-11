using CineQuebec.Application.Records.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Records.Films;

public record class RealisateurDto(Guid Id, string Prenom, string Nom) : PersonneDto(Id, Prenom, Nom);

internal static class RealisateurExtensions
{
    internal static RealisateurDto VersDto(this IRealisateur realisateur)
    {
        return new RealisateurDto(realisateur.Id, realisateur.Prenom, realisateur.Nom);
    }
}