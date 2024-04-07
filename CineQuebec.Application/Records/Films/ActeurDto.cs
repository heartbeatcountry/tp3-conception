using CineQuebec.Application.Records.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Films;

namespace CineQuebec.Application.Records.Films;

public record class ActeurDto(Guid Id, string Prenom, string Nom) : PersonneDto(Id, Prenom, Nom);

internal static class ActeurExtensions
{
    internal static ActeurDto VersDto(this IActeur acteur)
    {
        return new ActeurDto(acteur.Id, acteur.Prenom, acteur.Nom);
    }
}