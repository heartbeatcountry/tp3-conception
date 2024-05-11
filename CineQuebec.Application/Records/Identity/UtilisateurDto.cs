using CineQuebec.Application.Records.Abstract;
using CineQuebec.Domain.Interfaces.Entities.Utilisateur;

namespace CineQuebec.Application.Records.Identity;

public record class UtilisateurDto(
    Guid Id,
    string Prenom,
    string Nom
) : EntityDto(Id);

internal static class UtilisateurExtensions
{
    internal static UtilisateurDto VersDto(this IUtilisateur pUtilisateur)
    {
        return new UtilisateurDto(pUtilisateur.Id, pUtilisateur.Prenom, pUtilisateur.Nom);
    }
}