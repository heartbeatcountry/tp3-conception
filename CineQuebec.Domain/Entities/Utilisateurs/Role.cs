namespace CineQuebec.Domain.Entities.Utilisateurs;

public enum Role : byte
{
    Invite = 0,
    Utilisateur = 1 << 0,
    Administrateur = 1 << 1
}