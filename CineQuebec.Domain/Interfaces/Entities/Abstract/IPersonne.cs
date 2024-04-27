namespace CineQuebec.Domain.Interfaces.Entities.Abstract;

public interface IPersonne : IEntite
{
    string Prenom { get; }
    string Nom { get; }
    string NomComplet { get; }
    void SetPrenom(string prenom);
    void SetNom(string nom);
}